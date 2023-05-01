using AutoMapper;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Mappers.IdentityServer
{
    public class ClientProfile  :Profile
    {
        public ClientProfile()
        {
            CreateMap<EditClientUIModel, ClientModel>()
                .ForMember(x => x.RedirectUris, opt =>
                 {
                     opt.Condition(src => src.RedirectUris != null && src.RedirectUris != string.Empty);
                     opt.ConvertUsing(StringToListConverter.Instance, src => src.RedirectUris);
                 })
                .ForMember(x => x.PostLogoutRedirectUris, opt =>
                 {
                     opt.Condition(src => src.PostLogoutRedirectUris != null && src.PostLogoutRedirectUris != string.Empty);
                     opt.ConvertUsing(StringToListConverter.Instance, src => src.PostLogoutRedirectUris);
                 })
                .ForMember(x => x.AllowedCorsOrigins, opt =>
                {
                    opt.Condition(src => src.AllowedCorsOrigins != null && src.AllowedCorsOrigins != string.Empty);
                    opt.ConvertUsing(StringToHashSetConverter.Instance, src => src.AllowedCorsOrigins);
                })
                .ForMember(x => x.AllowedScopes, opt =>
                 {
                     opt.Condition(src => src.AllowedScopes != null);
                     opt.MapFrom(dist => dist.AllowedScopes);
                 });


            CreateMap<ClientDto, EditClientUIModel>()
                .ForMember(x => x.RedirectUris, opt =>
                 {
                     opt.Condition(src => src.RedirectUris != null);
                     opt.ConvertUsing(ClientRedirectUriConverter.Instance, src => src.RedirectUris);
                 })
                .ForMember(x => x.PostLogoutRedirectUris, opt =>
                {
                    opt.Condition(src => src.RedirectUris != null);
                    opt.ConvertUsing(ClientPostLogoutUriConverter.Instance, src => src.PostLogoutRedirectUris);
                })
                .ForMember(x => x.AllowedCorsOrigins, opt =>
                {
                    opt.Condition(src => src.RedirectUris != null);
                    opt.ConvertUsing(ClientCorsOriginConverter.Instance, src => src.AllowedCorsOrigins);
                })
                .ForMember(dist => dist.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes));

            CreateMap<ClientPropertyDto, PropertyUIModel>()
               .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ClientId))
               .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ClientScopeDto, string>().ConvertUsing(r => r.Scope);

        

        }
    }

    public class StringToListConverter : IValueConverter<string, List<string>>
    {
        public static StringToListConverter Instance = new StringToListConverter();
        public List<string>? Convert(string sourceMember, ResolutionContext context)
        {
            if(sourceMember == null)
            {
                return null;
            }

            return sourceMember.Split("\n").ToList();
        }
    }

    public class StringToHashSetConverter : IValueConverter<string, HashSet<string>>
    {
        public static StringToHashSetConverter Instance = new StringToHashSetConverter();

        public HashSet<string>? Convert(string sourceMember, ResolutionContext context)
        {
            if (sourceMember == null)
            {
                return null;
            }

            return sourceMember.Split("\n").ToHashSet();
        }
    }

    public class ClientRedirectUriConverter : IValueConverter<List<ClientRedirectUriDto>, string>
    {
        public static ClientRedirectUriConverter Instance = new ClientRedirectUriConverter();

        public string Convert(List<ClientRedirectUriDto> sourceMember, ResolutionContext context)
        {
           return sourceMember?.Select(x=> x.RedirectUri).JoinAsString("\n");
        }
    }
    public class ClientPostLogoutUriConverter : IValueConverter<List<ClientPostLogoutRedirectUriDto>, string>
    {
        public static ClientPostLogoutUriConverter Instance = new ClientPostLogoutUriConverter();

        public string Convert(List<ClientPostLogoutRedirectUriDto> sourceMember, ResolutionContext context)
        {
            return sourceMember?.Select(x => x.PostLogoutRedirectUri).JoinAsString("\n");
        }
    }
    public class ClientCorsOriginConverter : IValueConverter<List<ClientCorsOriginDto>, string>
    {
        public static ClientCorsOriginConverter Instance = new ClientCorsOriginConverter();

        public string Convert(List<ClientCorsOriginDto> sourceMember, ResolutionContext context)
        {
            return sourceMember?.Select(x => x.Origin).JoinAsString("\n");
        }
    }
}
