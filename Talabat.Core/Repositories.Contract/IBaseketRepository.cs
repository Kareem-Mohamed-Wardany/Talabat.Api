using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Repositories.Contract
{
    public interface IBaseketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
