using AutoMapper;
using Core.Dto;
using Core.Models;

namespace Core.Application.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Buyer, BuyerDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<CategoryDto, Category>();
            CreateMap<BuyerDto, Buyer>();
        }
    }
}
