using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace MicroStore.BuildingBlocks.AspNetCore.Security
{
    public class RequiredScopeAttribute : Attribute
    {
        public string? AllowedScope { get; set; }

        public RequiredScopeAttribute(string scope ) 
        {
            AllowedScope = scope;
        }
    }

     
}
