using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.TestBase;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests
{
    public class BaseTestFixture : ApplicationTestBase<IdentityServerTestModule>
    {
    }
}
