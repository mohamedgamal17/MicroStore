using AutoMapper;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));
        }
    }
}
