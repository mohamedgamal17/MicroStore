using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Identity.Application
{
    [ExposeServices(typeof(UserManager<ApplicationIdentityUser>), IncludeSelf = true)]
    public class ApplicationUserManager : UserManager<ApplicationIdentityUser> , ITransientDependency
    {
        public ApplicationUserManager(IUserStore<ApplicationIdentityUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationIdentityUser> passwordHasher, IEnumerable<IUserValidator<ApplicationIdentityUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationIdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationIdentityUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        public async Task<IdentityResult> UpdateAsync(ApplicationIdentityUser user, string? password = null)
        {
            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }


            if (password != null)
            {
                var passwordStore = GetPasswordStore();

                var result = await UpdatePasswordHash(user,password,true);

                if (!result.Succeeded)
                {
                    return result;
                }

            }

            return await UpdateUserAsync(user);

        }


        public async Task<IdentityResult> UpdateUserPasswordAsync(ApplicationIdentityUser user, string password)
        {
            var passwordStore = GetPasswordStore();

            var result = await UpdatePasswordHash(user, password,true);

            if (!result.Succeeded)
            {
                return result;
            }
            return await UpdateUserAsync(user);
        }

        private IUserPasswordStore<ApplicationIdentityUser> GetPasswordStore()
        {
            var cast = Store as IUserPasswordStore<ApplicationIdentityUser>;
            if (cast == null)
            {
                throw new NotSupportedException();
            }
            return cast;
        }
    }


}
