using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;
using System.Security.Cryptography;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure.Services
{
    public class CryptoServiceProvider : ICryptoServiceProvider, ITransientDependency
    {
        public async Task<string> GenerateRandomEncodedBase64Key(int lenght, string? perfix = null)
        {

            var randomBytes = await GenerateRandomKey(lenght);

            var randomText = Convert.ToBase64String(randomBytes);

            if(perfix != null && perfix != string.Empty) 
            {
                randomText = string.Format("{0}_{1}",perfix, randomBytes);
            }

            return randomText;
        }
        public Task<byte[]> GenerateRandomKey(int length )
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];

                rng.GetNonZeroBytes(randomBytes);

                return Task.FromResult( randomBytes);
            }
 
        }
    }
}
