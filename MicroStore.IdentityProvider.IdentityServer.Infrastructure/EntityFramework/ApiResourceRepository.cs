using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    [ExposeServices(new Type[] { typeof(IApiResourceRepository) , typeof(IRepository<ApiResource>)},IncludeSelf = true)]
    public class ApiResourceRepository : Repository<ApplicationConfigurationDbContext,ApiResource> , IApiResourceRepository , IScopedDependency
    {
        public ApiResourceRepository(ApplicationConfigurationDbContext DbContext) : base(DbContext)
        {
        }


        public async Task UpdateApiResourceAsync(ApiResource apiResource, CancellationToken cancellationToken = default)
        {


            await RemoveApiResourceRelationsAsync(apiResource.Id, cancellationToken);

            DbContext.Attach(apiResource);

            DbContext.Update(apiResource);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task RemoveApiResourceRelationsAsync(int apiResourceId , CancellationToken  cancellationToken)
        {

            var apiResourceScopes = await DbContext.Set<ApiResourceScope>().Where(x => x.ApiResourceId == apiResourceId)
                .ToListAsync(cancellationToken);
            DbContext.Set<ApiResourceScope>().RemoveRange(apiResourceScopes);

            var apiResourceClaims = await DbContext.Set<ApiResourceClaim>().Where(x=> x.ApiResourceId == apiResourceId)
                .ToListAsync(cancellationToken);
            DbContext.Set<ApiResourceClaim>().RemoveRange(apiResourceClaims);

            var apiResourceProperties = await DbContext.Set<ApiResourceProperty>().Where(x => x.ApiResourceId == apiResourceId)
                .ToListAsync(cancellationToken);
            DbContext.Set<ApiResourceProperty>().RemoveRange(apiResourceProperties);

        }
    }
}
