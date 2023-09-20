using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;
namespace MicroStore.Profiling.Application.Mappers
{
    public class AddressMapper : AutoMapper.Profile
    {
        public AddressMapper()
        {
            CreateMap<Address, AddressDto>();

            CreateMap<AddressModel, Address>();
        }
    }
}
