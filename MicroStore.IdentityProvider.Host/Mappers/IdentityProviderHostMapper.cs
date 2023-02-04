using AutoMapper;
using MicroStore.IdentityProvider.Host.Models;
using MicroStore.IdentityProvider.Identity.Application.Roles;
using MicroStore.IdentityProvider.Identity.Application.Users;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
namespace MicroStore.IdentityProvider.Host.Mappers
{
    public class IdentityProviderHostMapper : Profile
    {
        public IdentityProviderHostMapper()
        {
            CreateMap<ApiResourceModel, CreateApiResourceCommand>()
                .ReverseMap();

            CreateMap<ApiResourceModel, UpdateApiResourceCommand>()
                .ReverseMap();

            CreateMap<ApiScopeModel,CreateApiScopeCommand>()
                .ReverseMap();

            CreateMap<ApiScopeModel, UpdateApiScopeCommand>()
                .ReverseMap();

            CreateMap<ClientModel, CreateClientCommand>()
                .ReverseMap();

            CreateMap<ClientModel, UpdateClientCommand>()
                .ReverseMap();

            CreateMap<UserModel, CreateUserCommand>()
                .ReverseMap();

            CreateMap<UserModel, UpdateUserCommand>()
                .ReverseMap(); 

            CreateMap<RoleModel, CreateRoleCommand>()
                .ReverseMap();

            CreateMap<RoleModel, UpdateRoleCommand>()
                .ReverseMap();
        }
    }
}
