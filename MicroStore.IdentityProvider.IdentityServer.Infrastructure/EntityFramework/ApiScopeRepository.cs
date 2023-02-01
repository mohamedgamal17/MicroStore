using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    [ExposeServices(new Type[] { typeof(IApiScopeRepository), typeof(IRepository<ApiScope>) },IncludeSelf = true)]
    public class ApiScopeRepository : Repository<ApplicationConfigurationDbContext,  ApiScope>, IApiScopeRepository , IScopedDependency
    {
        public ApiScopeRepository(ApplicationConfigurationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task UpdateApiScopeAsync(ApiScope apiScope, CancellationToken cancellationToken = default)
        {

            await RemoveApiScopeRelationsAsync(apiScope.Id, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);

        }


        private async Task RemoveApiScopeRelationsAsync(int apiScopeId , CancellationToken cancellationToken = default)
        {

            var apiScopeClaims = await DbContext.Set<ApiScopeClaim>().Where(x => x.ScopeId == apiScopeId).ToListAsync(cancellationToken);
            DbContext.Set<ApiScopeClaim>().RemoveRange(apiScopeClaims);

            var apiScopeProperties = await DbContext.Set<ApiScopeProperty>().Where(x => x.ScopeId == apiScopeId).ToListAsync(cancellationToken);
            DbContext.Set<ApiScopeProperty>().RemoveRange(apiScopeProperties);
        }
    }
}
