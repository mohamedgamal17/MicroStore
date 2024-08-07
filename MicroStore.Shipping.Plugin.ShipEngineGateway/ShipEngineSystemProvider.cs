﻿using Microsoft.Extensions.Options;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Domain;
using ShipEngineSDK;
using ShipEngineSDK.Common;
using ShipEngineSDK.Common.Enums;
using ShipEngineSDK.CreateLabelFromShipmentDetails;
using ShipEngineSDK.GetRatesWithShipmentDetails;
using Volo.Abp;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineSystemProvider : IShipmentSystemProvider 
    {

        private readonly IShipmentRepository _shipmentRepository;

        private readonly IObjectMapper _objectMapper;


        private readonly ShippingSystem _system;
        public ShipEngineSystemProvider( IShipmentRepository shipmentRepository, IObjectMapper objectMapper,  IOptions<ShippingSystemOptions> options)
        {
            _shipmentRepository = shipmentRepository;
            _objectMapper = objectMapper;
            _system = options.Value.Systems.Single(x => x.Name == ShipEngineConst.Provider);
        }

        public async Task<Result<ShipmentDto>> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {

            return await WrappResponseResult( async () =>
            {
                var clinet = await GetShipEngineClinet(cancellationToken);

                var shipment = await _shipmentRepository.RetriveShipment(shipmentId, cancellationToken);

                var label = await clinet.CreateLabelFromRate(new ShipEngineSDK.CreateLabelFromRate.Params
                {
                    RateId = model.ShipmentRateId,
                    LabelDownloadType = ShipEngineSDK.CreateLabelFromRate.LabelDownloadType.Url,
                    LabelLayout = LabelLayout.FourBySix
                });

                shipment!.BuyShipmentLabel(label.LabelId!, label.TrackingNumber!);

                await _shipmentRepository.UpdateAsync(shipment);

                return _objectMapper.Map<MicroStore.Shipping.Domain.Entities.Shipment, ShipmentDto>(shipment);

            });
        
        }

        public async Task<Result<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel addressFrom , AddressModel addressTo , List<ShipmentItemEstimationModel>  items, CancellationToken cancellationToken = default)
        {

            return await WrappResponseResult(async () =>
            {
                var clinet = await GetShipEngineClinet();

                var carriersResult = await clinet.ListCarriers();

                var estimaedRate = PrepareEstimateRate(addressFrom,addressTo,items);

                estimaedRate.CarrierIds = carriersResult.Carriers.Select(x=> x.CarrierId).ToArray();

                var result =  await clinet.EstimateRate(estimaedRate);

                return PrepareEstimateRateDto(result);
            });

          
        }

        public async Task<Result<ShipmentDto>> Fullfill(string shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(async () =>
            {
                var clinet = await GetShipEngineClinet();

                var shipment = await _shipmentRepository.RetriveShipment(shipmentId, cancellationToken)!;

                ShipEngineShipment shipEngineShipment = new ShipEngineShipment()
                {
                    ExternalShipmentId = shipment.Id.ToString(),
                    ExternalOrderId = shipment.OrderId,
                    Items = _objectMapper.Map<List<MicroStore.Shipping.Domain.Entities.ShipmentItem>, List<ShipEngineShipmentItem>>(shipment.Items),
                    ShipFrom = _objectMapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, ShipEngineAddress>(model.AddressFrom.AsAddress()),
                    ShipTo = _objectMapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, ShipEngineAddress>(model.AddressTo.AsAddress()),
                    Packages = new List<Package>
                {
                    new Package
                    {
                        Dimensions  = ConvertDimension(model.Package.Dimension.AsDimension()),
                        Weight = ConvertWeight(model.Package.Weight.AsWeight())
                    }

                },
                };

                var result = await clinet.CreateShipment(shipEngineShipment);

                shipment.Fullfill(_system.Name, result.ShipmentId);

                await _shipmentRepository.UpdateAsync(shipment);


                return _objectMapper.Map<MicroStore.Shipping.Domain.Entities.Shipment, ShipmentDto>(shipment);
            });
                     
        }

        public async Task<Result<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(async () =>
            {
                var shipment = await _shipmentRepository.RetriveShipment(shipmentId, cancellationToken)!;

                var clinet = await GetShipEngineClinet();


                var carriers = await clinet.ListCarriers();

                var rateOptions = new RateOptions
                {
                    CarrierIds = carriers?.Carriers.Select(x => x.CarrierId).ToList()
                };


                var result = await clinet.GetRatesWithShipmentDetails(new ShipEngineSDK.GetRatesWithShipmentDetails.Params { ShipmentId = shipment.ShipmentExternalId, RateOptions = rateOptions });

                var rates = result.RateResponse.Rates.Select(x => new ShipmentRateDto
                {
                    Id = x.RateId,

                    Amount = new MoneyDto
                    {
                        Currency = x.ShippingAmount.Currency.ToString(),
                        Value = (double)((x.ShippingAmount?.Amount + x.InsuranceAmount?.Amount + x.OtherAmount?.Amount) ?? 0),
                    },

                    CarrierId = x.CarrierId,

                    Days = x.DeliveryDays,

                    ServiceLevel = new ServiceLevelDto
                    {
                        Code = x.ServiceCode,
                        Name = x.CarrierFriendlyName
                    },

                }).ToList();

                return rates;
                 

            });
                       
        }

        public async Task<Result<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {

            return await WrappResponseResult(async () =>
            {
                var clinet = await GetShipEngineClinet();

                var address = _objectMapper.Map<AddressModel, ShipEngineAddress>(addressModel);

                var result = await clinet.ValidateAddresses(new List<ShipEngineAddress> { address });

                var addressValidationResult = result.First();

                return new AddressValidationResultModel
                {
                    IsValid = addressValidationResult.Status
                        != ShipEngineSDK.ValidateAddresses.AddressValidationResult.Error,

                    Messages = addressValidationResult.Messages.Select(x => new AddressValidationMessages
                    {
                        Type = x.Type.ToString(),
                        Code = x.Code,
                        Message = x.Message
                    }).ToList()
                };

            });

        }






        private async Task <Result<T>> WrappResponseResult<T>(Func<Task<T>> func)
        {
            try
            {
               return await func();

            }
            catch (ShipEngineException ex)
            {
                return new Result<T>(new UserFriendlyException(ex.Message));

            }
          
        }
        private Weight ConvertWeight(MicroStore.Shipping.Domain.ValueObjects.Weight weight)
        {

            return weight.Unit switch
            {
                MicroStore.Shipping.Domain.ValueObjects.WeightUnit.Gram => new Weight
                {
                    Value = weight.Value,
                    Unit = WeightUnit.Gram
                },

                MicroStore.Shipping.Domain.ValueObjects.WeightUnit.Pound => new Weight
                {
                    Value = weight.Value,
                    Unit = WeightUnit.Pound
                },

                MicroStore.Shipping.Domain.ValueObjects.WeightUnit.KiloGram => new Weight
                {
                    Value = weight.Value,
                    Unit = WeightUnit.Kilogram
                },

                _ => throw new InvalidOperationException("Unsupported weight system unit")
            };
      
        }

        private Dimensions ConvertDimension(MicroStore.Shipping.Domain.ValueObjects.Dimension dimension)
        {
            var convertedDimensions = MicroStore.Shipping.Domain.ValueObjects.Dimension.ConvertToInch(dimension);

            return new Dimensions
            {
                Height = convertedDimensions.Height,
                Length = convertedDimensions.Length,
                Width = convertedDimensions.Width,
                Unit = DimensionUnit.Inch
            };
        }

        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private EstimateRate PrepareEstimateRate(AddressModel addressFrom , AddressModel addressTo, List<ShipmentItemEstimationModel> items)
        {
            return new EstimateRate
            {
                FromCountryCode = addressFrom.CountryCode,
                FromCityLocality = addressFrom.City,
                FromStateProvince = addressFrom.State,
                FromPostalCode = addressFrom.PostalCode,
                ToCountryCode = addressTo.CountryCode,
                ToCityLocality = addressTo.City,
                ToStateProvince = addressTo.State,
                ToPostalCode = addressTo.PostalCode,
                Weight = EstimatePacakgeWeight(items)
            };
        }


        private Weight EstimatePacakgeWeight(List<ShipmentItemEstimationModel> items)
        {
            var itemsWeight = items.Select(x => x.Weight.AsWeight() * x.Quantity).ToList();

            var estimatedWeight = itemsWeight
                 .Select(x => MicroStore.Shipping.Domain.ValueObjects.Weight.ConvertToPound(x))
                 .Aggregate((t1, t2) => t1 + t2);

            return new Weight { Value = estimatedWeight.Value, Unit = WeightUnit.Pound };
        }

        private List<EstimatedRateDto> PrepareEstimateRateDto(List<EstimatedRateResult> result)
        {
            return result.Where(x=> x.ValidationStatus != "invalid").Select(x => new EstimatedRateDto
            {
                Name = x.ServiceType,
                EstaimatedDays =  x.DeliveryDays ?? 0  ,
                ShippingDate = x.ShipDate,
                Money = new MoneyDto
                {
                    Value =  (x.InsuranceAmount?.Amount + x.OtherAmount?.Amount + x.ShippingAmount?.Amount) ?? 0,  
                    Currency = x.ShippingAmount?.Currency.ToString(),
                }

            }).ToList();
        }


        private async Task<ShipEngineClinet> GetShipEngineClinet(CancellationToken cancellationToken = default)
        {
            return  ShipEngineClinet.Create(_system.Configuration);
        }

      
    }
}
