using AutoMapper;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;

namespace MicroStore.Geographic.Application.Mappers
{
    public class StateProvinceProfile : Profile
    {
        public StateProvinceProfile()
        {
            CreateMap<StateProvinceModel, StateProvince>();

            CreateMap< StateProvince,StateProvinceDto>();
        }
    }
}
