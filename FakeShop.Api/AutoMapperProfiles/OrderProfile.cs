using AutoMapper;
using FakeShop.Api.ApiModels;
using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();
        }
    }
}
