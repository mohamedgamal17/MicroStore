using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.Profiling.Application.Dtos;
using MicroStore.Profiling.Application.Models;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Profiling.Application.Services;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Paging;
using Volo.Abp.Uow;
namespace MicroStore.Profiling.Host.Grpc
{
    public class ProfileGrpcService : ProfileService.ProfileServiceBase ,IUnitOfWorkEnabled
    {
        private readonly IProfileCommandService _profileCommandService;

        private readonly IProfileQueryService _profileQueryService;
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        private readonly IServiceProvider _serviceProvider;
        public ProfileGrpcService(IProfileCommandService profileCommandService, IProfileQueryService profileQueryService, IAbpLazyServiceProvider lazyServiceProvider, IServiceProvider serviceProvider)
        {
            _profileCommandService = profileCommandService;
            _profileQueryService = profileQueryService;
            LazyServiceProvider = lazyServiceProvider;
            _serviceProvider = serviceProvider;
        }



        public override async Task<ProfileResponse> Create(ProfileRequest request, ServerCallContext context)
        {
            var model = PrepareCreateProfileModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _profileCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProfileResponse(result.Value);
        }

        public override async Task<ProfileResponse> Update(ProfileRequest request, ServerCallContext context)
        {
            var model = PrepareUpdateProfileModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _profileCommandService.UpdateAsync(request.UserId,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProfileResponse(result.Value);
        }

        public override async Task<ProfileListResponse> GetList(ProfileListRequest request, ServerCallContext context)
        {
            var pagingParams = new PagingQueryParams
            {
                Length = request.Length,
                Skip = request.Skip
            };

            var result = await _profileQueryService.ListAsync(pagingParams);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProfileListResponse(result.Value);
        }

       
        public override async Task<ProfileResponse> GetByUserId(GetProfileByUserIdRequest request, ServerCallContext context)
        {
            var result = await _profileQueryService.GetAsync(request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProfileResponse(result.Value);
        }

        public override async Task<AddressResponse> CreateAddress(CreateAddressRequest request, ServerCallContext context)
        {
            var model = PrepareAddressModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _profileCommandService.CreateAddressAsync(request.UserId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareAddressResponse(result.Value);
        }

        public override async Task<AddressResponse> UpdateAddress(UpdateAddressReqeust request, ServerCallContext context)
        {
            var model = PrepareAddressModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _profileCommandService.UpdateAddressAsync(request.UserId, request.Id,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareAddressResponse(result.Value);
        }

        public override async Task<EmptyResponse> DeleteAddress(DeleteAddressReqeust request, ServerCallContext context)
        {
            var result = await _profileCommandService.RemoveAddressAsync(request.UserId, request.AddressId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return new EmptyResponse();
        }
        public override async Task<AddressListResponse> GetAddressList(GetAddressListRequest request, ServerCallContext context)
        {
            var result = await _profileQueryService.ListAddressesAsync(request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PrepareAddressListResponse(result.Value);
        }

        public override async Task<AddressResponse> GetAddressById(GetAddressByIdReqeuest request, ServerCallContext context)
        {
            var result = await _profileQueryService.GetAddressAsync(request.UserId, request.AddressId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareAddressResponse(result.Value);
        }
        private CreateProfileModel PrepareCreateProfileModel(ProfileRequest request)
        {
            var model = new CreateProfileModel
            {
                UserId = request.UserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Avatar = request.Avatar,
                Phone = request.Phone,
                BirthDate = request.BirthDate.ToDateTime(),
                Gender = request.Gender.ToString()
            };

            if(request.Addresses != null && request.Addresses.Items.Count > 0)
            {
                model.Addresses = request.Addresses.Items.Select(x => new AddressModel
                {
                    Name = x.Name,
                    CountryCode = x.CountryCode,
                    State = x.StateProvince,
                    City = x.City,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    Phone = x.Phone,
                    PostalCode = x.PostalCode,
                    Zip = x.Zip
                }).ToList();
            }

            return model;
        }

        private ProfileModel PrepareUpdateProfileModel(ProfileRequest request)
        {
            var model =  new ProfileModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Avatar = request.Avatar,
                Phone = request.Phone,
                BirthDate = request.BirthDate.ToDateTime(),
                Gender = request.Gender.ToString()
            };

            if (request.Addresses != null && request.Addresses.Items.Count > 0)
            {
                model.Addresses = request.Addresses.Items.Select(x => new AddressModel
                {
                    Name = x.Name,
                    CountryCode = x.CountryCode,
                    State = x.StateProvince,
                    City = x.City,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    Phone = x.Phone,
                    PostalCode = x.PostalCode,
                    Zip = x.Zip
                }).ToList();
            }

            return model;
        }

        private ProfileListResponse PrepareProfileListResponse(PagedResult<ProfileDto> paged)
        {
            var response = new ProfileListResponse
            {
                Skip = paged.Skip,
                Length = paged.Lenght,
                TotalCount = paged.TotalCount
            };

            foreach (var item in paged.Items)
            {
                response.Items.Add(PrepareProfileResponse(item));
            }

            return response;
        }
        private ProfileResponse PrepareProfileResponse(ProfileDto profile)
        {
            var response = new ProfileResponse
            {
                Id = profile.Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Avatar = profile.Avatar,
                Gender = System.Enum.Parse<Gender>(profile.Gender),
                UserId = profile.UserId,
                BirthDate = Timestamp.FromDateTime(profile.BirthDate.ToUniversalTime()),
                Phone = profile.Phone,
                CreatedAt = Timestamp.FromDateTime(profile.CreationTime.ToUniversalTime()),
                ModifiedAt = profile.LastModificationTime?.ToUniversalTime().ToTimestamp()
            };

            if(profile.Addresses != null)
            {
                profile.Addresses.ForEach(add =>
                {
                    response.Addresses.Add(PrepareAddressResponse(add));
                });
            }

            return response;
        }

        private AddressModel PrepareAddressModel(CreateAddressRequest request)
        {
            return new AddressModel
            {
                Name = request.Name,
                CountryCode = request.CountryCode,
                State = request.StateProvince,
                City = request.City,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                Phone = request.Phone,
                PostalCode = request.PostalCode,
                Zip = request.Zip,

            };
        }

        private AddressModel PrepareAddressModel(UpdateAddressReqeust request)
        {
            return new AddressModel
            {
                Name = request.Name,
                CountryCode = request.CountryCode,
                State = request.StateProvince,
                City = request.City,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                Phone = request.Phone,
                PostalCode = request.PostalCode,
                Zip = request.Zip
            };
        }

        private AddressListResponse PrepareAddressListResponse(List<AddressDto> addresses)
        {
            var response = new AddressListResponse();

            addresses.ForEach(add => response.Items.Add(PrepareAddressResponse(add)));

            return response;
        }
        private AddressResponse PrepareAddressResponse(AddressDto address)
        {
            return new AddressResponse
            {
                Id = address.Id,
                Name = address.Name,
                Phone = address.Phone,
                CountryCode = address.CountryCode,
                City = address.City,
                StateProvince = address.State,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
                Zip = address.Zip,
            };
        }
        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }
        private IValidator<T>? ResolveValidator<T>()
        {
            return _serviceProvider.GetService<IValidator<T>>();
        }
    }
}
