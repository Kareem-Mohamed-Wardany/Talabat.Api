using Talabat.Core;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Application.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly IBaseketRepository _baksetRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration config,
            IBaseketRepository baksetRepo,
            IUnitOfWork unitOfWork
            )
        {
            _config = config;
            _baksetRepo = baksetRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string baksetId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:Secretkey"];
            var bakset = await _baksetRepo.GetBasketAsync(baksetId);
            if (bakset is null) return null;

            var ShippingPrice = 0m;
            if (bakset.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync(bakset.DeliveryMethodId.Value);
                ShippingPrice = deliveryMethod.Cost;
                bakset.ShippingPrice = ShippingPrice;
            }

            if (bakset.Items?.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in bakset.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;

                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(bakset.PaymentIntentId)) // Create New one
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(bakset.Items.Sum(item => item.Price * 100 * item.Quantity) + (ShippingPrice * 100)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);
                bakset.PaymentIntentId = paymentIntent.Id;
                bakset.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update existing one
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(bakset.Items.Sum(item => item.Price * 100 * item.Quantity) + (ShippingPrice * 100)),
                };
                await paymentIntentService.UpdateAsync(bakset.PaymentIntentId, options);

            }

            await _baksetRepo.UpdateBasketAsync(bakset);
            return bakset;
        }
        public async Task<Order> UpdatePaymentIntentToSucceedOrFailed(string intentId, bool IsSucceed)
        {
            var spec = new OrderWithPaymentIntentSpecifications(intentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (IsSucceed)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
