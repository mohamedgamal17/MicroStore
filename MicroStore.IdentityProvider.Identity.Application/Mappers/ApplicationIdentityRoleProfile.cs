using AutoMapper;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Mappers
{
    public class ApplicationIdentityRoleProfile : Profile
    {
        public ApplicationIdentityRoleProfile()
        {
            CreateMap<ApplicationIdentityRole, IdentityRoleDto>();
               

        }
    }
}
