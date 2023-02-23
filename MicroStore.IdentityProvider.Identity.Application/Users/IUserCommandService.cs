using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public interface IUserCommandService : IApplicationService
    {
        Task<UnitResultV2<IdentityUserDto>> CreateUserAsync(UserModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<IdentityUserDto>> UpdateUserAsync(string userId, UserModel model, CancellationToken cancellationToken = default);

    }
}
