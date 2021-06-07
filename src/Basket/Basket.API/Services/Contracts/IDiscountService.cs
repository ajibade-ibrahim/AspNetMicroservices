using System.Threading.Tasks;
using Discount.gRPC.Protos;

namespace Basket.API.Services.Contracts
{
    public interface IDiscountService
    {
        Task<CouponModel> GetDiscount(string productName);
    }
}