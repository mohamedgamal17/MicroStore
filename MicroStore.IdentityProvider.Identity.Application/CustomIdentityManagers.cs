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

        public override Task<IdentityResult> AddToRolesAsync(ApplicationIdentityUser user, IEnumerable<string> roles)
        {
            return base.AddToRolesAsync(user, roles);
        }

        public override Task<IdentityResult> AddPasswordAsync(ApplicationIdentityUser user, string password)
        {
            return base.AddPasswordAsync(user, password);
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

    [ExposeServices(typeof(RoleManager<ApplicationIdentityRole>), IncludeSelf = true)]
    public class ApplicationRoleManager : RoleManager<ApplicationIdentityRole> , ITransientDependency
    {
         
        public ApplicationRoleManager(IRoleStore<ApplicationIdentityRole> store, IEnumerable<IRoleValidator<ApplicationIdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationIdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

    }
}
