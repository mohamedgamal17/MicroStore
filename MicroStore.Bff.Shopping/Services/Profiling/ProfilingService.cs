using Google.Protobuf.WellKnownTypes;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Grpc.Profiling;
using MicroStore.Bff.Shopping.Models.Common;
using MicroStore.Bff.Shopping.Models.Profiling;
using MicroStore.Bff.Shopping.Services.Geographic;
namespace MicroStore.Bff.Shopping.Services.Profiling
{
    public class ProfilingService
    {
        private readonly Grpc.Profiling.ProfileService.ProfileServiceClient _profilingClient;

        private readonly CountryService _countryService;
        public ProfilingService(ProfileService.ProfileServiceClient profilingClient, CountryService countryService)
        {
            _profilingClient = profilingClient;
            _countryService = countryService;
        }

        public async Task<PagedList<UserProfile>> ListAsync(int skip , int length , CancellationToken cancellationToken = default)
        {
            ProfileListRequest request = new ProfileListRequest
            {
                Skip = skip,
                Length = length
            };

            var response = await _profilingClient.GetListAsync(request);

            var tasks = response.Items.Select(PrepareUserProfile);

            var users = await Task.WhenAll(tasks);

            var result = new PagedList<UserProfile>
            {
                Items = users,
                Skip = response.Skip,
                TotalCount = response.TotalCount,
                Length = response.Length
            };

            return result;
        }

        public async Task<UserProfile> GetUserAsync(string userId , CancellationToken cancellationToken = default)
        {
            var request = new GetProfileByUserIdRequest { UserId = userId };

            var response = await _profilingClient.GetByUserIdAsync(request);

            return await PrepareUserProfile(response);
        }

        public async Task<List<Address>> ListUserAddressAsync(string userId , CancellationToken cancellationToken = default)
        {
            var request = new GetAddressListRequest { UserId = userId };

            var response = await _profilingClient.GetAddressListAsync(request);

            var tasks = response.Items.Select(PrepareAddress);

            var addresses = await Task.WhenAll(tasks);

            return addresses.ToList();
        }

        public async Task<Address> GetUserAddressAsync(string userId, string addressId , CancellationToken cancellationToken = default)
        {
            var request = new GetAddressByIdReqeuest { UserId = userId, AddressId = addressId };

            var response = await _profilingClient.GetAddressByIdAsync(request);

            return await PrepareAddress(response);
        }

        
        public async Task<UserProfile> CreateAsync(string userId,  UserProfileModel model , CancellationToken cancellationToken = default)
        {
            var request = PrepareProfileReqeuest(userId, model);

            var response = await _profilingClient.CreateAsync(request);

            return await PrepareUserProfile(response);
        }

        public async Task<UserProfile> UpdateAsync(string userId, UserProfileModel model, CancellationToken cancellationToken= default)
        {
            var request = PrepareProfileReqeuest(userId,model);

            var response = await _profilingClient.CreateAsync(request);

            return await PrepareUserProfile(response);
        }

        public async Task<Address> CreateAddressAsync(string userId , AddressModel model , CancellationToken cancellationToken = default)
        {
            var request = new CreateAddressRequest
            {
                UserId = userId,
                Name = model.Name,
                CountryCode = model.Country,
                StateProvince = model.State,
                City = model.City,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Phone = model.Phone,
                PostalCode = model.PostalCode,
                Zip = model.Zip
            };

            var response = await _profilingClient.CreateAddressAsync(request);

            return await PrepareAddress(response);
        }

        public async Task<Address> UpdateAddressAsync(string userId, string addressId , AddressModel model, CancellationToken cancellationToken = default)
        {
            var request = new UpdateAddressReqeust
            {
                Id = addressId,
                UserId = userId,
                Name = model.Name,
                CountryCode = model.Country,
                StateProvince = model.State,
                City = model.City,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Phone = model.Phone,
                PostalCode = model.PostalCode,
                Zip = model.Zip
            };

            var response = await _profilingClient.UpdateAddressAsync(request);

            return await PrepareAddress(response);
        }

        public async Task RemoveAddressAsync(string userId , string addressId , CancellationToken cancellationToken = default)
        {
            var request = new DeleteAddressReqeust { UserId = userId, AddressId = addressId };

            await _profilingClient.DeleteAddressAsync(request);
        }

        private async Task<UserProfile> PrepareUserProfile(ProfileResponse response)
        {
            var userProfile = new UserProfile
            {
                Id = response.Id,
                UserId = response.UserId,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Avatar = response.Avatar,
                BirthDate = response.BirthDate.ToDateTime(),
                Gender = (Data.Profiling.Gender)response.Gender,
                Phone = response.Phone,
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };

            if(response.Addresses.Count > 0)
            {
                userProfile.Addresses = new List<Address>();

                List<Task<Address>> tasks = new List<Task<Address>>();

                foreach (var addressResponse in response.Addresses)
                {
                    tasks.Add(PrepareAddress(addressResponse));
                }

                 var addresses =  await Task.WhenAll(tasks);


                userProfile.Addresses.AddRange(addresses);
            }

            return userProfile;
        }



        private async Task<Address> PrepareAddress(AddressResponse response)
        {
            var country = await _countryService.GetByCodeAsync(response.CountryCode);
            var stateProvince =  country.StateProvinces.Single(x => x.Abbreviation == response.StateProvince);

            return new Address
            {
                Id = response.Id,
                Name = response.Name,
                Country = new Data.Common.AddressCountry
                {
                    Name = country.Name,
                    TwoIsoCode = country.TwoLetterIsoCode,
                    ThreeIsoCode = country.TwoLetterIsoCode,
                    NumericIsoCode = country.NumericIsoCode
                },
                State = new Data.Common.AddressStateProvince
                {
                    Name = stateProvince.Name,
                    Abbreviation = stateProvince.Abbreviation
                },
                City = response.City,
                AddressLine1 = response.AddressLine1,
                AddressLine2 = response.AddressLine2,
                Phone = response.Phone,
                PostalCode = response.PostalCode,
                Zip = response.Zip
            };
        }


        private ProfileRequest PrepareProfileReqeuest(string userId,UserProfileModel model)
        {
            var request = new ProfileRequest
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Avatar = model.Avatar,
                Phone = model.Phone,
                BirthDate = model.BirthDate.ToUniversalTime().ToTimestamp(),
                Gender = (Grpc.Profiling.Gender)model.Gender
            };


            if(model.Addresses != null)
            {
                var addressList = new AddressReqeustList();

                foreach (var address in model.Addresses)
                {
                    addressList.Items.Add(new AddressReqeuest
                    {
                        Name = address.Name,
                        CountryCode = address.Country,
                        StateProvince = address.State,
                        City = address.City,
                        AddressLine1 = address.AddressLine1,
                        AddressLine2 = address.AddressLine2,
                        Phone = address.Phone,
                        PostalCode = address.PostalCode,
                        Zip = address.Zip
                    });
                }

                request.Addresses = addressList;
            }

            return request;
        }
    }
}
