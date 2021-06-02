using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories.Contracts
{
    public interface IBasketRepository
    {
        Task DeleteBasket(string username);
        Task<ShoppingCart> GetBasket(string username);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
    }
}