using MicroStore.IdentityProvider.Identity.Application;
using MicroStore.IdentityProvider.Identity.Application.Common;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.Identity.Infrastructure.Extensions;
using Volo.Abp.DependencyInjection;

namespace MicroStore.IdentityProvider.Identity.Infrastructure.EntityFramework
{
    public class IdentityUserRepository : IIdentityUserRepository, ITransientDependency
    {
        private readonly ApplicationUserManager _userManager;

        public IdentityUserRepository(ApplicationUserManager userManager)
        {
            _userManager = userManager;

        }

        public async Task<ApplicationIdentityUser> CreateAsync(ApplicationIdentityUser user, string? password = null, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.CreateAsync(user);

            result.ThorwIfInvalidResult();

            if (password != null)
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, password);

                passwordResult.ThorwIfInvalidResult();
            }

            return user;
        }
        public async Task<ApplicationIdentityUser> UpdateAsync(ApplicationIdentityUser user, string? password = null, CancellationToken cancellationToken = default)
        {
            var result = await _userManager.UpdateAsync(user);

            result.ThorwIfInvalidResult();

            if (password != null)
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, password);

                passwordResult.ThorwIfInvalidResult();

            }

            return user;
        }

        public async Task<ApplicationIdentityUser?> FindByEmail(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        public async Task<ApplicationIdentityUser?> FindById(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<ApplicationIdentityUser?> FindByUserName(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }
    }
}
