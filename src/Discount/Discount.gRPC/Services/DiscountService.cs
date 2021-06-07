using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discount.API.Entities;
using Discount.API.Repositories.Contracts;
using Discount.gRPC.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.gRPC.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _repository;

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(
            DeleteDiscountRequest request,
            ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = deleted
            };
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }

            _logger.LogInformation(
                "Discount is retrieved for ProductName : {productName}, Amount : {amount}",
                coupon.ProductName,
                coupon.Amount);

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            return _mapper.Map<CouponModel>(coupon);
        }
    }
}