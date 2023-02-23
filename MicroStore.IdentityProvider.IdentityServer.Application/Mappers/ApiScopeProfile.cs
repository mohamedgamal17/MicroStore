using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ApiScopeProfile : Profile
    {
        public ApiScopeProfile()
        {
            CreateMap<ApiScope, ApiScopeDto>()
                .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims));


            CreateMap<ApiScopeProperty, ApiScopePropertyDto>();
            CreateMap<ApiScopeClaim, ApiScopeClaimDto>();
            
 
            CreateMap<ApiScopeModel, ApiScope>()
                 .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims))
                 .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties));


            CreateMap<string, ApiScopeClaim>()
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src));

            CreateMap<PropertyModel, ApiScopeProperty>()
                .ForMember(x => x.Key, opt => opt.MapFrom(c => c.Key))
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value));
        }
    }
}
