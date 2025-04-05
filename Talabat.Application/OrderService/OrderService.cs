using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Order_Specs;

namespace Talabat.Application.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBaseketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepo;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        ///private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBaseketRepository BasketRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            ///IGenericRepository<Product> productRepo,
            ///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            ///IGenericRepository<Order> orderRepo
            )
        {
            _basketRepo = BasketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string BasketID, int deliveryMethodId, OrderAddress shippingAddress)
        {
            // 1.Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(BasketID);

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);

                }
            }

            // 3. Calculate SubTotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();


            var existingOrder = await orderRepo.GetWithSpecAsync(new OrderWithPaymentIntentSpecifications(basket?.PaymentIntentId));
            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketID);

            }

            // 5. Create Order

            var order = new Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = shippingAddress,
                DeliveryMethodId = deliveryMethodId,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                Subtotal = subtotal,
                PaymentIntentId = basket.PaymentIntentId

            };
            orderRepo.Add(order);

            // 6. Save To Database [TODO]
            var res = await _unitOfWork.CompleteAsync();
            if (res <= 0) return null;
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await ordersRepo.GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
            => await _unitOfWork.Repository<Order>().GetWithSpecAsync(new OrderSpecifications(orderId, buyerEmail));
    }
}
