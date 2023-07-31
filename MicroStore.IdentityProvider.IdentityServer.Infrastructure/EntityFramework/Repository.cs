using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.Linq.Expressions;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        protected readonly ApplicationConfigurationDbContext DbContext;

        public Repository(ApplicationConfigurationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().AnyAsync(expression, cancellationToken);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Set<TEntity>().Remove(entity);

            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
            
            return entity;
        }

        //private async Task RemoveClientRelationsAsync(Client client, ClientModel model)
        //{
        //    //Remove old allowed scopes

        //    if()

        //    var clientScopes = await DbContext.ClientScopes.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientScopes.RemoveRange(clientScopes);

        //    //Remove old grant types
        //    var clientGrantTypes = await DbContext.ClientGrantTypes.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientGrantTypes.RemoveRange(clientGrantTypes);

        //    //Remove old redirect uri
        //    var clientRedirectUris = await DbContext.ClientRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientRedirectUris.RemoveRange(clientRedirectUris);

        //    //Remove old client cors
        //    var clientCorsOrigins = await DbContext.ClientCorsOrigins.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientCorsOrigins.RemoveRange(clientCorsOrigins);

        //    //Remove old client id restrictions
        //    var clientIdPRestrictions = await DbContext.ClientIdPRestrictions.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientIdPRestrictions.RemoveRange(clientIdPRestrictions);

        //    //Remove old client post logout redirect
        //    var clientPostLogoutRedirectUris = await DbContext.ClientPostLogoutRedirectUris.Where(x => x.Client.Id == client.Id).ToListAsync();
        //    DbContext.ClientPostLogoutRedirectUris.RemoveRange(clientPostLogoutRedirectUris);

        //    //Remove old client claims
        //    if (updateClientClaims)
        //    {
        //        var clientClaims = await DbContext.ClientClaims.Where(x => x.Client.Id == client.Id).ToListAsync();
        //        DbContext.ClientClaims.RemoveRange(clientClaims);
        //    }

        //    //Remove old client properties
        //    if (updateClientProperties)
        //    {
        //        var clientProperties = await DbContext.ClientProperties.Where(x => x.Client.Id == client.Id).ToListAsync();
        //        DbContext.ClientProperties.RemoveRange(clientProperties);
        //    }
        //}

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().SingleAsync(expression, cancellationToken);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return DbContext.Set<TEntity>().SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbContext.Set<TEntity>().Update(entity);

            await DbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<TEntity> FirstAsync(CancellationToken cancellationToken = default)
        {
           return await DbContext.Set<TEntity>().FirstAsync(cancellationToken);
        }
        public async Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<TEntity>().FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<TEntity> Query()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }
    }
}
