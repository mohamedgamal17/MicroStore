using MicroStore.BuildingBlocks.Results;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;

namespace MicroStore.Profiling.Application.Services
{
    public interface IProfileCommandService
    {
        Task<Result<ProfileDto>> CreateAsync(ProfileModel model , CancellationToken cancellationToken);
        Task<Result<ProfileDto>> UpdateAsync(string userId , ProfileModel model , CancellationToken cancellationToken);
        Task<Result<AddressDto>> CreateAddressAsync(string userId , AddressModel model , CancellationToken cancellationToken);
        Task<Result<AddressDto>> UpdateAddressAsync(string userId, string addressId , AddressModel model, CancellationToken cancellationToken);
        Task<Result<Unit>> RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken);
    }
}
