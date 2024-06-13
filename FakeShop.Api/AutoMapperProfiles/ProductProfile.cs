using AutoMapper;
using FakeShop.Api.ApiModels;
using FakeShop.Api.Domain.Entities;

namespace FakeShop.Api.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
