using AutoMapper;
using Discount.API.Entities;
using Discount.gRPC.Protos;

namespace Discount.gRPC.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}