using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Rates;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Shipping.Host.Grpc
{
    public class RateGrpcService : RateService.RateServiceBase
    {
        private readonly IRateApplicationService _rateApplicationService;

        public RateGrpcService(IRateApplicationService rateApplicationService)
        {
            _rateApplicationService = rateApplicationService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<RateListResponse> Estimate(EstimateShippingRateRequest request, ServerCallContext context)
        {
            var model = PrepareEstimateRateModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _rateApplicationService.EstimateRate(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PrepareRateListResponse(result.Value);
        }


        private EstimatedRateModel PrepareEstimateRateModel(EstimateShippingRateRequest request)
        {
            var model = new EstimatedRateModel
            {
                Address = PrepareAddressReqeust(request.Address),
                Items = request.Items.Select(x => new ShipmentItemEstimationModel
                {
                    Name = x.Name,
                    Sku = x.Sku,
                    Quantity = x.Quantity,
                    UnitPrice = new MoneyDto
                    {
                        Value = x.UnitPrice?.Value ?? 0,
                        Currency = x.UnitPrice?.Currency
                    },
                    Weight = new WeightModel
                    {
                        Unit = x.Weight.Unit.ToString(),
                        Value = x.Weight.Value
                    }
                }).ToList()
               
            };

            return model;
        }
        private AddressModel PrepareAddressReqeust(AddressRequest request)
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
        private RateListResponse PrepareRateListResponse(List<EstimatedRateDto> items)
        {
            var response = new RateListResponse();

            foreach (var item in items)
            {
                response.Items.Add(PrepareRateResponse(item));
            }

            return response;
        }
        private RateResponse PrepareRateResponse(EstimatedRateDto rate)
        {
            var response = new RateResponse
            {
                Name = rate.Name,
                Money = new MoneyResponse
                {
                    Value = rate.Money?.Value ?? 0,
                    Currency = rate.Money?.Currency,
                },
                EstimatedDays = rate.EstaimatedDays
            };

            if(rate.ShippingDate != null)
            {
                response.ShippingDate = rate.ShippingDate.Value.ToTimestamp();
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
