using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;

namespace MicroStore.IdentityProvider.Identity.Application.Common
{
    public interface IIdentityUserRepository
    {
        Task<ApplicationIdentityUser> CreateAsync(ApplicationIdentityUser user, string? password = null, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityUser> UpdateAsync(ApplicationIdentityUser user, string? password = null, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityUser?> FindById(string userId, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityUser?> FindByEmail(string email, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityUser?> FindByUserName(string userName, CancellationToken cancellationToken = default);

    }
}
