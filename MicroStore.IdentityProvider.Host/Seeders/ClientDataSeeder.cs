using Duende.IdentityServer.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Host.Seeders
{
    [ExposeServices(typeof(IDataSeeder))]
    public class ClientDataSeeder : IDataSeeder , ITransientDependency
    {
        public async Task SeedAsync(DataSeederContext context, CancellationToken cancellationToken = default)
        {
            var clientRepository = context.ServiceProvider.GetRequiredService<IClientRepository>();

            var m2mClient = await  clientRepository.SingleOrDefaultAsync(x => x.ClientId == "4d573b56d23c41fd920711e77e66ae62",cancellationToken);

            if (m2mClient == null)
            {
                m2mClient = new Duende.IdentityServer.EntityFramework.Entities.Client
                {
                    ClientName = "M2MClient",
                    ClientId = "4d573b56d23c41fd920711e77e66ae62",

                    ClientSecrets = new List<Duende.IdentityServer.EntityFramework.Entities.ClientSecret>
                    {
                        new Duende.IdentityServer.EntityFramework.Entities.ClientSecret
                        {
                            Value = "a0100c2d864a47608b4660c7b332ae41".Sha512()
                        },
                    },

                    AllowedGrantTypes = new List<Duende.IdentityServer.EntityFramework.Entities.ClientGrantType>
                    {
                        new Duende.IdentityServer.EntityFramework.Entities.ClientGrantType
                        {
                            GrantType = OpenIdConnectGrantTypes.ClientCredentials
                        }
                    },

                };

                await clientRepository.InsertAsync(m2mClient, cancellationToken);
            }
        }
    }
}
