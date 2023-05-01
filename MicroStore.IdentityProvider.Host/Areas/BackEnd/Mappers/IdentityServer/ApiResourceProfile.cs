using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Mappers.IdentityServer
{
    public class ApiResourceProfile : Profile
    {
        public ApiResourceProfile()
        {
            CreateMap<ApiResourceUIModel, ApiResourceModel>();

            CreateMap<ApiResourceDto, ApiResourceUIModel>()
                .ForMember(x => x.Scopes, opt => opt.MapFrom(c => c.Scopes))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims));

            CreateMap<ApiResourceScopeDto, string>()
                .ConvertUsing(r => r.Scope);

            CreateMap<ApiResourceClaimDto, string>()
                .ConvertUsing(r => r.Type);

            CreateMap<ApiResourcePropertyDto, PropertyUIModel>()
                .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ApiResourceId))
                .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));


            CreateMap<ApiResourceSecretUIModel, SecretModel>();

       
        }
    }
}
