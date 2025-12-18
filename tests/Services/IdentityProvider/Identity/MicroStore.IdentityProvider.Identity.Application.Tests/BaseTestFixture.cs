using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.TestBase;
namespace MicroStore.IdentityProvider.Identity.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : ApplicationTestBase<IdentityApplicationTestModule>
    {
        



        protected void ThrowIfFailureResult(IdentityResult identityResult)
        {
            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(identityResult.Errors.JoinAsString("\n"));
            }
        }
    }
}