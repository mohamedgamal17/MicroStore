using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Geographic.Application.Countries;
using Google.Protobuf.WellKnownTypes;
namespace MicroStore.Geographic.Host.Grpc
{
    public class CountryGrpcService : CountryService.CountryServiceBase
    {
        private readonly ICountryApplicationService _countryApplicationService;

        public CountryGrpcService(ICountryApplicationService countryApplicationService)
        {
            _countryApplicationService = countryApplicationService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<CountryResponse> Create(CreateCountryRequest request, ServerCallContext context)
        {
            var model = PrepareCountryMode(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _countryApplicationService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareCountryResponse(result.Value);
        }

        public override async Task<CountryResponse> Update(UpdateCountryRequest request, ServerCallContext context)
        {
            var model = PrepareCountryMode(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _countryApplicationService.UpdateAsync(request.Id,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareCountryResponse(result.Value);
        }

        public override async Task<CountryListResponse> GetList(CountryListRequest request, ServerCallContext context)
        {
            var result = await _countryApplicationService.ListAsync();

            return PrepareCountryListResponse(result);
        }

        public override async Task<CountryResponse> GetById(GetCountryByIdRequest request, ServerCallContext context)
        {
            var result = await _countryApplicationService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareCountryResponse(result.Value);

        }

        public override async Task<CountryResponse> GetByCode(GetCountryByCodeRequest request, ServerCallContext context)
        {
            var result = await _countryApplicationService.GetByCodeAsync(request.Code);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareCountryResponse(result.Value);
        }

        private CountryModel PrepareCountryMode(CreateCountryRequest request)
        {
            return new CountryModel
            {
                Name = request.Name,
                TwoLetterIsoCode = request.TwoLetterIsoCode,
                ThreeLetterIsoCode = request.ThreeLetterIsoCode,
                NumericIsoCode = request.NumericIsoCode
            };
        }

        private CountryModel PrepareCountryMode(UpdateCountryRequest request)
        {
            return new CountryModel
            {
                Name = request.Name,
                TwoLetterIsoCode = request.TwoLetterIsoCode,
                ThreeLetterIsoCode = request.ThreeLetterIsoCode,
                NumericIsoCode = request.NumericIsoCode
            };
        }

        private CountryResponse PrepareCountryResponse(CountryDto country)
        {
            var respoonse = new CountryResponse
            {
                Id = country.Id,
                Name = country.Name,
                TwoLetterIsoCode = country.TwoLetterIsoCode,
                ThreeLetterIsoCode = country.ThreeLetterIsoCode,
                NumericIsoCode = country.NumericIsoCode,
                CreatedAt = country.CreationTime.ToUniversalTime().ToTimestamp(),
                ModifiedAt = country.LastModificationTime?.ToUniversalTime().ToTimestamp()
            };


            country.StateProvinces.ForEach(state =>
            {
                var stateResponse = new StateProvinceResponse
                {
                    Id = state.Id,
                    Name = state.Name,
                    Abbrevation = state.Abbreviation,
                    CreatedAt = state.CreationTime.ToUniversalTime().ToTimestamp(),
                    ModifiedAt = state.LastModificationTime?.ToUniversalTime().ToTimestamp()
                };

                respoonse.States.Add(stateResponse);
            });

            return respoonse;
        }

        private CountryListResponse PrepareCountryListResponse(List<CountryListDto> countries)
        {
            var response = new CountryListResponse();
            countries.ForEach(x =>
            {
                var country = new CountryResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    TwoLetterIsoCode = x.TwoLetterIsoCode,
                    ThreeLetterIsoCode = x.ThreeLetterIsoCode,
                    NumericIsoCode = x.NumericIsoCode,
                    CreatedAt = x.CreationTime.ToUniversalTime().ToTimestamp(),
                    ModifiedAt = x.LastModificationTime?.ToUniversalTime().ToTimestamp()
                };


                response.Items.Add(country);
            });


            return response;
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }
        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
