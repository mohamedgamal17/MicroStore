using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;

namespace MicroStore.Profiling.Application.Services
{
    public interface IProfileCommandService
    {
        Task<Result<ProfileDto>> CreateAsync(CreateProfileModel model , CancellationToken cancellationToken = default);
        Task<Result<ProfileDto>> UpdateAsync(string userId , ProfileModel model , CancellationToken cancellationToken= default);
        Task<Result<AddressDto>> CreateAddressAsync(string userId , AddressModel model , CancellationToken cancellationToken = default);
        Task<Result<AddressDto>> UpdateAddressAsync(string userId, string addressId , AddressModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken = default);
    }
}
