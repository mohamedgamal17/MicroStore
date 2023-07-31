using AutoMapper;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Mappers.IdentityServer
{
    public class ApiResourceProfile : Profile
    {
        public ApiResourceProfile()
        {
            CreateMap<CreateOrEditApiResourceModel, ApiResourceModel>();

            CreateMap<ApiResourceDto, CreateOrEditApiResourceModel>()
                .ForMember(x => x.Scopes, opt => opt.MapFrom(c => c.Scopes))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims));

            CreateMap<ApiResourceScopeDto, string>()
                .ConvertUsing(r => r.Scope);

            CreateMap<ApiResourceClaimDto, string>()
                .ConvertUsing(r => r.Type);

            CreateMap<ApiResourcePropertyDto, PropertyViewModel>()
                .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ApiResourceId))
                .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));


            CreateMap<CreateApiResourceSecretModel, SecretModel>();


        }
    }
}
