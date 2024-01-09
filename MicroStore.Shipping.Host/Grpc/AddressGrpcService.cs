using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Addresses;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Shipping.Host.Grpc
{
    public class AddressGrpcService : AddressService.AddressServiceBase
    {
        private readonly IAddressApplicationService _addressApplicationService;

        public AddressGrpcService(IAddressApplicationService addressApplicationService)
        {
            _addressApplicationService = addressApplicationService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<AddressValidationResultResponse> Validate(AddressRequest request, ServerCallContext context)
        {
            var model = PrepareAddressModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _addressApplicationService.ValidateAddress(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return PrepareAddressValidationResponse(result.Value);
        }

        private AddressModel PrepareAddressModel(AddressRequest request)
        {
            return new AddressModel
            {
                Name = request.Name,
                CountryCode = request.CountryCode,
                City = request.City,
                State = request.StateProvince,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                PostalCode = request.PostalCode,
                Phone = request.Phone,
                Zip = request.Zip
            };
        }

        private AddressValidationResultResponse PrepareAddressValidationResponse(AddressValidationResultModel result)
        {
            var response = new AddressValidationResultResponse
            {
                IsValid = result.IsValid
            };

            if(result.Messages != null)
            {
                foreach (var item in result.Messages)
                {
                    var message = new AddressValidationResponse
                    {
                        Type = item.Type,
                        Message = item.Message,
                        Code = item.Code
                    };

                    response.Messages.Add(message);
                }
              
            }

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
