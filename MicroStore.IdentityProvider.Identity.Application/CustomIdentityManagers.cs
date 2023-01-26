using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Resources;
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
    }

    [ExposeServices(typeof(RoleManager<ApplicationIdentityRole>), IncludeSelf = true)]
    public class ApplicationRoleManager : RoleManager<ApplicationIdentityRole> , ITransientDependency
    {
         
        public ApplicationRoleManager(IRoleStore<ApplicationIdentityRole> store, IEnumerable<IRoleValidator<ApplicationIdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationIdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

    }
}
