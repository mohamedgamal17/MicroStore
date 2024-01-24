using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Profiling.Application.Domain;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Profiling.Application.Services
{
    public class ProfileCommandService : ProfilingApplicationService, IProfileCommandService
    {
        private readonly IRepository<Profile> _profileRepository;

        public ProfileCommandService(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public async Task<Result<ProfileDto>> CreateAsync(CreateProfileModel model, CancellationToken cancellationToken)
        {

            bool isUserHasProfile = await _profileRepository.AnyAsync(x => x.UserId == model.UserId);

            if (isUserHasProfile)
            {
                return new Result<ProfileDto>(new UserFriendlyException("User already has profile"));
            }

            var profile = ObjectMapper.Map<CreateProfileModel, Profile>(model);

            await _profileRepository.InsertAsync(profile);

            return ObjectMapper.Map<Profile,ProfileDto>(profile);
        }

        public async Task<Result<ProfileDto>> UpdateAsync(string userId, ProfileModel model, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken); 

            if(profile == null)
            {
                return new Result<ProfileDto>(new EntityNotFoundException(typeof(Profile), userId));
            }

            ObjectMapper.Map(model,profile);

            await _profileRepository.UpdateAsync(profile);

            return ObjectMapper.Map<Profile, ProfileDto>(profile);
        }

        public async Task<Result<AddressDto>> CreateAddressAsync(string userId, AddressModel model, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);


            if (profile == null)
            {
                return new Result<AddressDto>(new EntityNotFoundException(typeof(Profile), userId));
            }

            var address = ObjectMapper.Map<AddressModel, Address>(model);

            profile.Addresses.Add(address);

            await _profileRepository.UpdateAsync(profile);

            return ObjectMapper.Map<Address, AddressDto>(address);

        }

        public async Task<Result<AddressDto>> UpdateAddressAsync(string userId, string addressId, AddressModel model, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (profile == null)
            {
                return new Result<AddressDto>(new EntityNotFoundException(typeof(Profile), userId));
            }

            var address = profile.Addresses.SingleOrDefault(x => x.Id == addressId);

            if(address == null)
            {
                return new Result<AddressDto>(new EntityNotFoundException(typeof(Address), addressId));
            }

            ObjectMapper.Map(model, address);

            await _profileRepository.UpdateAsync(profile);

            return ObjectMapper.Map<Address, AddressDto>(address);
        }

        public async Task<Result<Unit>> RemoveAddressAsync(string userId, string addressId, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (profile == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Profile), userId));
            }

            var address = profile.Addresses.SingleOrDefault(x => x.Id == addressId);

            if (address == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Address), addressId));
            }

            profile.Addresses.Remove(address);

            await _profileRepository.UpdateAsync(profile);

            return Unit.Value;

        }
    }
}
