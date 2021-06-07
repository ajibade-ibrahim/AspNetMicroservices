using System;
using System.Threading.Tasks;
using Basket.API.Services.Contracts;
using Discount.gRPC.Protos;

namespace Basket.API.Services.gRPC
{
    public class DiscountService : IDiscountService
    {
        public DiscountService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService =
                discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest
            {
                ProductName = productName
            };
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}