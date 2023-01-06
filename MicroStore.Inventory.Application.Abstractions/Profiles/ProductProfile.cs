using AutoMapper;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Abstractions.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(c => c.Id));
        }
    }
}
