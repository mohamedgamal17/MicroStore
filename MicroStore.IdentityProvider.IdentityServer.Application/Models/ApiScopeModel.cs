using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiScopeModel
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<PropertyModel>? Properties { get; set; }
    }

   
}
