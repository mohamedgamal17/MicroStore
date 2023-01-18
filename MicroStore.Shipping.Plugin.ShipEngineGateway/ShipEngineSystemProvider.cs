using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Domain;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using ShipEngineSDK;
using ShipEngineSDK.Common;
using ShipEngineSDK.Common.Enums;
using ShipEngineSDK.CreateLabelFromShipmentDetails;
using ShipEngineSDK.GetRatesWithShipmentDetails;
using System.Net;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    [ExposeServices(typeof(IShipmentSystemProvider),IncludeDefaults =true,IncludeSelf =true)]
    public class ShipEngineSystemProvider : IShipmentSystemProvider , ITransientDependency
    {
        public string SystemName => ShipEngineConst.SystemName;

        private readonly IShipmentRepository _shipmentRepository;

        private readonly IObjectMapper _objectMapper;

        private readonly ISettingsRepository _settingsRepository;

        public ShipEngineSystemProvider( IShipmentRepository shipmentRepository, IObjectMapper objectMapper,  ISettingsRepository settingsRepository)
        {
            _shipmentRepository = shipmentRepository;
            _objectMapper = objectMapper;
             _settingsRepository = settingsRepository;
        }

        public async Task<ResponseResult<ShipmentDto>> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {

            return await WrappResponseResult(HttpStatusCode.OK, async () =>
            {
                var clinet = await GetShipEngineClinet(cancellationToken);

                var shipment = await _shipmentRepository.RetriveShipmentByExternalId(externalShipmentId, cancellationToken);

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

        public async Task<ResponseResult<ListResultDto<EstimatedRateDto>>> EstimateShipmentRate(EstimatedRateModel model , CancellationToken cancellationToken = default)
        {

            return await WrappResponseResult(HttpStatusCode.OK, async () =>
            {
                var clinet = await GetShipEngineClinet();

                var carriersResult = await clinet.ListCarriers();

                var estimaedRate = PrepareEstimateRate(model);

                estimaedRate.CarrierIds = carriersResult.Carriers.Select(x=> x.CarrierId).ToArray();

                var result =  await clinet.EstimateRate(estimaedRate);

                return new ListResultDto<EstimatedRateDto>(PrepareEstimateRateDto(result).AsReadOnly());
            });

          
        }

        public async Task<ResponseResult<ShipmentDto>> Fullfill(Guid shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(HttpStatusCode.OK, async () =>
            {
                var clinet = await GetShipEngineClinet();

                var shipment = await _shipmentRepository.RetriveShipment(shipmentId, cancellationToken);

                if (shipment == null)
                {
                    throw new EntityNotFoundException(typeof(MicroStore.Shipping.Domain.Entities.Shipment), shipmentId);
                }

                ShipEngineShipment shipEngineShipment = new ShipEngineShipment()
                {
                    ExternalShipmentId = shipment.Id.ToString(),
                    ExternalOrderId = shipment.OrderId,
                    Items = _objectMapper.Map<List<MicroStore.Shipping.Domain.Entities.ShipmentItem>, List<ShipEngineShipmentItem>>(shipment.Items),
                    ShipFrom = _objectMapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, ShipEngineAddress>(model.AddressFrom.AsAddress()),
                    ShipTo = _objectMapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, ShipEngineAddress>(shipment.Address),
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

                shipment.Fullfill(SystemName, result.ShipmentId);

                await _shipmentRepository.UpdateAsync(shipment);


                return _objectMapper.Map<MicroStore.Shipping.Domain.Entities.Shipment, ShipmentDto>(shipment);
            });
                     
        }

        public async Task<ResponseResult<ListResultDto<ShipmentRateDto>>> RetriveShipmentRates(string externalShipmentId, CancellationToken cancellationToken = default)
        {
            return await WrappResponseResult(HttpStatusCode.OK ,async () =>
            {
                var clinet = await GetShipEngineClinet();

                var settings = await _settingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName) ?? new ShipEngineSettings();


                var carriers = await clinet.ListCarriers();

                var rateOptions = new RateOptions
                {
                    CarrierIds = carriers?.Carriers.Select(x => x.CarrierId).ToList()
                };


                var result = await clinet.GetRatesWithShipmentDetails(new ShipEngineSDK.GetRatesWithShipmentDetails.Params { ShipmentId = externalShipmentId, RateOptions = rateOptions });

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

                }).ToList().AsReadOnly();

                return new ListResultDto<ShipmentRateDto>(rates);
                 

            });
                       
        }

        public async Task<ResponseResult<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default)
        {

            return await WrappResponseResult(HttpStatusCode.OK, async () =>
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




        private async Task<ResponseResult<T>> WrappResponseResult<T>(HttpStatusCode statusCode,Func<Task<T>> func)
        {
            return await WrappResponseResult(async () =>
            {
                var result = await func();

                return ResponseResult.Success<T>((int)statusCode, result);
            });
        }

        private async Task <ResponseResult<T>> WrappResponseResult<T>(Func<Task<ResponseResult<T>>> func)
        {
            try
            {
               return await func();

            }
            catch (ShipEngineException ex)
            {
                var errorInfo = new ErrorInfo
                {
                    Type = ex.ErrorType.ToString(),
                    Source = ex.Source,
                    Message = ex.Message,
                };

                return ResponseResult.Failure<T>((int)HttpStatusCode.BadRequest, errorInfo);

            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                var errorInfo = new ErrorInfo
                {
                    Message = "Ship Engine Api is not availabe now"
                };

                return ResponseResult.Failure<T>((int)HttpStatusCode.BadRequest, errorInfo);
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

            return dimension.Unit switch
            {
                MicroStore.Shipping.Domain.ValueObjects.DimensionUnit.Inch => new Dimensions
                {
                    Height = dimension.Height,
                    Length = dimension.Length,
                    Width = dimension.Width,
                    Unit = DimensionUnit.Inch

                },

                MicroStore.Shipping.Domain.ValueObjects.DimensionUnit.CentiMeter => new Dimensions
                {
                    Height = dimension.Height,
                    Length = dimension.Length,
                    Width = dimension.Width,
                    Unit = DimensionUnit.Inch

                },

                _ => throw new InvalidOperationException("Unsupported dimension system unit")
            };
          
        }

        public Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private EstimateRate PrepareEstimateRate(EstimatedRateModel model)
        {
            return new EstimateRate
            {
                FromCountryCode = model.AddressFrom.CountryCode,
                FromCityLocality = model.AddressFrom.City,
                FromStateProvince = model.AddressFrom.State,
                FromPostalCode = model.AddressFrom.PostalCode,
                ToCountryCode = model.AddressTo.CountryCode,
                ToCityLocality = model.AddressTo.City,
                ToStateProvince = model.AddressTo.State,
                ToPostalCode = model.AddressTo.PostalCode,
                Weight = EstimatePacakgeWeight(model.Items)
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
            return result.Select(x => new EstimatedRateDto
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
            var settings = await _settingsRepository.TryToGetSettings<ShipEngineSettings>(ShipEngineConst.SystemName, cancellationToken) ?? new ShipEngineSettings();

            return new ShipEngineClinet(settings);
        }

      
    }
}
