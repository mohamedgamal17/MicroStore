using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Mappers.IdentityServer
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<EditClientModel, ClientModel>()
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
                 })
                .ForMember(x => x.AllowedGrantTypes, opt =>
                {
                    opt.ConvertUsing(AllowedGrantConverter.Instance, src => src.AllowedGrantTypes);
                });


            CreateMap<ClientDto, EditClientModel>()
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
                .ForMember(dist => dist.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes))
                .ForMember(dist => dist.AllowedGrantTypes, opt =>
                {
                    opt.ConvertUsing(AllowedGrantConverter.Instance, src=> src.AllowedGrantTypes);
                });

            CreateMap<ClientPropertyDto, PropertyViewModel>()
               .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ClientId))
               .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ClientPropertyDto, PropertyViewModel>()
             .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ClientId))
             .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ClientScopeDto, string>().ConvertUsing(r => r.Scope);




        }
    }

    public class StringToListConverter : IValueConverter<string, List<string>>
    {
        public static StringToListConverter Instance = new StringToListConverter();
        public List<string> Convert(string sourceMember, ResolutionContext context)
        {
            if (sourceMember == null)
            {
                return new List<string>();
            }

            return sourceMember.Split("\n").ToList();
        }
    }

    public class StringToHashSetConverter : IValueConverter<string, HashSet<string>>
    {
        public static StringToHashSetConverter Instance = new StringToHashSetConverter();

        public HashSet<string> Convert(string sourceMember, ResolutionContext context)
        {
            if (sourceMember == null)
            {
                return new HashSet<string>();
            }

            return sourceMember.Split("\n").ToHashSet();
        }
    }

    public class ClientRedirectUriConverter : IValueConverter<List<ClientRedirectUriDto>, string>
    {
        public static ClientRedirectUriConverter Instance = new ClientRedirectUriConverter();

        public string Convert(List<ClientRedirectUriDto> sourceMember, ResolutionContext context)
        {
            return sourceMember?.Select(x => x.RedirectUri).JoinAsString("\n") ?? string.Empty;
        }
    }
    public class ClientPostLogoutUriConverter : IValueConverter<List<ClientPostLogoutRedirectUriDto>, string>
    {
        public static ClientPostLogoutUriConverter Instance = new ClientPostLogoutUriConverter();

        public string Convert(List<ClientPostLogoutRedirectUriDto> sourceMember, ResolutionContext context)
        {
            return sourceMember?.Select(x => x.PostLogoutRedirectUri).JoinAsString("\n") ?? string.Empty;
        }
    }
    public class ClientCorsOriginConverter : IValueConverter<List<ClientCorsOriginDto>, string>
    {
        public static ClientCorsOriginConverter Instance = new ClientCorsOriginConverter();

        public string Convert(List<ClientCorsOriginDto> sourceMember, ResolutionContext context)
        {
            return sourceMember?.Select(x => x.Origin).JoinAsString("\n") ?? string.Empty;
        }
    }

    public class AllowedGrantConverter : IValueConverter<List<ClientGrantTypeDto>, List<SelectListItem>>,
        IValueConverter<List<SelectListItem>, List<string>>     
    {
        public static AllowedGrantConverter Instance = new AllowedGrantConverter();
        public List<SelectListItem> Convert(List<ClientGrantTypeDto> sourceMember, ResolutionContext context)
        {
            var allowedGrants = sourceMember?.Select(x => x.GrantType).ToList() ?? new List<string>();
            return IdentityServerConsts.AllowrdGrants.Select(kvp => new SelectListItem
            {
                Text = kvp.Key,
                Value = kvp.Value,
                Selected = allowedGrants.Contains(kvp.Value) 
            }).ToList();
        }

        public List<string> Convert(List<SelectListItem> sourceMember, ResolutionContext context)
        {
            return sourceMember.Where(x => x.Selected)
                 .Select(x => x.Value)
                 .ToList();
        }
    }
}
