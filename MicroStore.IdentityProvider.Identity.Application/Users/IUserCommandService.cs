using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public interface IUserCommandService : IApplicationService
    {
        Task<Result<IdentityUserDto>> CreateUserAsync(UserModel model, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> UpdateUserAsync(string userId, UserModel model, CancellationToken cancellationToken = default);
    }
}
