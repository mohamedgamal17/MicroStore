using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
namespace MicroStore.IdentityProvider.Identity.Application.Common
{
    public interface IRoleRepository
    {
        Task<ApplicationIdentityRole> CreateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityRole> UpdateAsync(ApplicationIdentityRole role, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityRole?> FindById(string roleId, CancellationToken cancellationToken = default);
        Task<ApplicationIdentityRole?> FindByName(string? name, CancellationToken cancellationToken = default);


    }
}
