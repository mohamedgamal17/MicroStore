using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.Identity.Application.Domain;
using System.Globalization;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.Services
{
    [ExposeServices(typeof(IUserStore<ApplicationIdentityUser>), IncludeSelf =  true)]
    public class ApplicationUserStore : UserStore<ApplicationIdentityUser, ApplicationIdentityRole, ApplicationIdentityDbContext, string, ApplicationIdentityUserClaim, ApplicationIdentityUserRole, ApplicationIdentityUserLogin, ApplicationIdentityUserToken, ApplicationIdentityRoleClaim>, ITransientDependency
    {
        public ApplicationUserStore(ApplicationIdentityDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {

        }


        public override async Task AddToRoleAsync(ApplicationIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default)
        {

            cancellationToken.ThrowIfCancellationRequested();

            ThrowIfDisposed();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ArgumentException("Parameter cannot be null or empty", nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);

            if (roleEntity == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, $"Role with name : {normalizedRoleName} is not exist", normalizedRoleName));
            }


            user.AddUserRole(roleEntity);
        }




    }

    [ExposeServices(typeof(IRoleStore<ApplicationIdentityRole>), IncludeSelf = true)]
    public class ApplicationRoleStore : RoleStore<ApplicationIdentityRole, ApplicationIdentityDbContext, string, ApplicationIdentityUserRole, ApplicationIdentityRoleClaim>, ITransientDependency
    {
        public ApplicationRoleStore(ApplicationIdentityDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {

        }

       
    }


}
