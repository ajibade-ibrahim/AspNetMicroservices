using System.Threading.Tasks;
using Discount.API.Entities;

namespace Discount.API.Repositories.Contracts
{
    public interface IDiscountRepository
    {
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
        Task<Coupon> GetDiscount(string productName);
        Task<bool> UpdateDiscount(Coupon coupon);
    }
}