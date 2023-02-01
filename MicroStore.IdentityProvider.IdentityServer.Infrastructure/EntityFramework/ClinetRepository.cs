using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework
{
    [ExposeServices(new Type[] { typeof(IClinetRepository), typeof(IRepository<Client>) }, IncludeSelf = true)]
    public class ClinetRepository : Repository<ApplicationConfigurationDbContext, Client>, IClinetRepository, IScopedDependency 
    {
        public ClinetRepository(ApplicationConfigurationDbContext dbContext) : base(dbContext)
        {
        }

        public  async Task UpdateClinetAsync(Client client, CancellationToken cancellationToken = default)
        {

            await RemoveClientRelationsAsync(client.Id);

            DbContext.Attach(client);

            DbContext.Update(client);

            await DbContext.SaveChangesAsync(cancellationToken);
        }


        

        private async Task RemoveClientRelationsAsync(int clinetId  )
        {


            var clientScopes = await DbContext.Set<ClientScope>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientScope>().RemoveRange(clientScopes);

            var clientGrantTypes = await DbContext.Set<ClientGrantType>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientGrantType>().RemoveRange(clientGrantTypes);

            var clientRedirectUris = await DbContext.Set<ClientRedirectUri>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientRedirectUri>().RemoveRange(clientRedirectUris);


            var clientCorsOrigins = await DbContext.Set<ClientCorsOrigin>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientCorsOrigin>().RemoveRange(clientCorsOrigins);

            var clientIdPRestrictions = await DbContext.Set<ClientIdPRestriction>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientIdPRestriction>().RemoveRange(clientIdPRestrictions);

            var clientPostLogoutRedirectUris = await DbContext.Set<ClientPostLogoutRedirectUri>().Where(x => x.ClientId == clinetId).ToListAsync();
            DbContext.Set<ClientPostLogoutRedirectUri>().RemoveRange(clientPostLogoutRedirectUris);


            var clientProperties = await DbContext.Set<ClientProperty>().Where(x => x.Client.Id == clinetId).ToListAsync();
            DbContext.Set<ClientProperty>().RemoveRange(clientProperties);
        }   

    }
}
