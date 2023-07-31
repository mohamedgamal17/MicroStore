using Microsoft.AspNetCore.Identity;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Fakes
{
    public class FakeUserClaimPrincibalFactory: IUserClaimsPrincipalFactory<ApplicationIdentityUser> , ITransientDependency 
    {
        public Task<ClaimsPrincipal> CreateAsync(ApplicationIdentityUser user)
        {
            return Task.FromResult(new ClaimsPrincipal());
        }
    }
}
