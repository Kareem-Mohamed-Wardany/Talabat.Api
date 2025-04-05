using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string baksetId);
        Task<Order> UpdatePaymentIntentToSucceedOrFailed(string intentId, bool IsSucceed);

    }
}
