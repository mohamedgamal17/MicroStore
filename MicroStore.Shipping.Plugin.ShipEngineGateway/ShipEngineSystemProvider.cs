using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Domain;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using ShipEngineSDK.Common;
using ShipEngineSDK.Common.Enums;
using ShipEngineSDK.GetRatesWithShipmentDetails;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineSystemProvider : IShipmentSystemProvider , ITransientDependency
    {
        public string SystemName => ShipEngineConst.SystemName;

        private readonly ShipEngineClinet _shipEngineClinet;

        private readonly IShipmentRepository _shipmentRepository;

        private readonly IMapper _mapper;

        private readonly ShipEngineSettings _settings;

        public ShipEngineSystemProvider(ShipEngineClinet shipEngineClinet, IShipmentRepository shipmentRepository, IMapper mapper, ShipEngineSettings settings)
        {
            _shipEngineClinet = shipEngineClinet;
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
            _settings = settings;
        }

        public async Task<ShipmentDto> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentRepository.RetriveShipmentByExternalId(externalShipmentId, cancellationToken);

            var label = await _shipEngineClinet.CreateLabelFromRate(new ShipEngineSDK.CreateLabelFromRate.Params
            {
                RateId = model.ShipmentRateId,
                LabelDownloadType = ShipEngineSDK.CreateLabelFromRate.LabelDownloadType.Url,
            });

            shipment.BuyShipmentLabel(label.LabelId!, label.TrackingNumber!);

            await _shipmentRepository.UpdateAsync(shipment);

            return _mapper.Map<MicroStore.Shipping.Domain.Entities.Shipment, ShipmentDto>(shipment);

        }

        public async Task<List<EstimatedRateDto>> EstimateShipmentRate(EstimatedRateModel model)
        {
            var result  = await _shipEngineClinet.EstimateRate(PrepareEstimateRate(model));

            return PrepareEstimateRateDto(result.Rates.ToList());
        }

        public async Task<ShipmentFullfilledDto> Fullfill(Guid shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentRepository.RetriveShipment(shipmentId, cancellationToken);

            if (shipment == null)
            {
                throw new EntityNotFoundException(typeof(MicroStore.Shipping.Domain.Entities.Shipment), shipmentId);
            }

            Shipment shipEngineShipment = new Shipment()
            {

                CarrierId = model.CarrierId,
                ExternalShipmentId = shipment.Id.ToString(),
                ExternalOrderId = shipment.OrderId,
                Items = _mapper.Map<List<MicroStore.Shipping.Domain.Entities.ShipmentItem>, List<ShipmentItem>>(shipment.Items) ,
                ShipFrom = _mapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, Address>(model.AddressFrom.AsAddress()) ,  
                ShipTo =  _mapper.Map<MicroStore.Shipping.Domain.ValueObjects.Address, Address>(shipment.Address),
                Weight = ConvertWeight(model.Package.Weight.AsWeight()),
                Packages = new List<ShipmentPackage>
                {
                    new ShipmentPackage
                    {
                        Dimensions  = ConvertDimension(model.Package.Dimension.AsDimension()),
                        Weight = ConvertWeight(model.Package.Weight.AsWeight())
                    }

                },
            };

            var result = await _shipEngineClinet.CreateShipment(shipEngineShipment);

            shipment.Fullfill(SystemName, result.ShipmentId);

            await _shipmentRepository.UpdateAsync(shipment);

            return new ShipmentFullfilledDto
            {
                ShipmentId = shipment.Id,
                ExternalShipmentId = shipment.ShipmentExternalId,
                Items = _mapper.Map<List<MicroStore.Shipping.Domain.Entities.ShipmentItem>, List<ShipmentItemDto>>(shipment.Items),
                AddressFrom =_mapper.Map<Address,AddressDto>(result.ShipFrom),
                AddressTo = _mapper.Map<Address, AddressDto>(result.ShipTo)
            };
        }

        public Task<bool> IsActive(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(ShipEngineConst.Active);
        }

        public async Task<List<ShipmentRateDto>> RetriveShipmentRates(string externalShipmentId)
        {
            var result = await _shipEngineClinet.GetRatesWithShipmentDetails(new Params { ShipmentId = externalShipmentId });

            return result.RateResponse.Rates.Select(x => new ShipmentRateDto
            {
                RateId = x.RateId,

                Amount = new MoneyDto
                {
                    Currency = x.ShippingAmount.Currency.ToString(),
                    Value = (double)(x.ShippingAmount.Amount + x.InsuranceAmount.Amount + x.OtherAmount.Amount),
                },

                CarrierId= x.CarrierId,

                Days = x.DeliveryDays,

                ServiceLevel = new ServiceLevelDto
                {
                    Code = x.ServiceCode,
                    Name  =x.CarrierFriendlyName
                },         
         
            }).ToList();
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
            return Task.FromResult(_settings.Carriers.Select(x => new CarrierModel
            {
                CarrierId = x.CarrierId,
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsActive = x.IsEnabled
            }).ToList());
        }

        private EstimateRate PrepareEstimateRate(EstimatedRateModel model)
        {
            return new EstimateRate
            {
                CarrierIds = _settings.Carriers.Select(x => x.CarrierId).ToArray(),
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

        private List<EstimatedRateDto> PrepareEstimateRateDto(List<ShipmentRate> result)
        {
            return result.Select(x => new EstimatedRateDto
            {
                Name = x.ServiceType,
                EstaimatedDays = x.DeliveryDays,
                Money = new MoneyDto
                {
                    Value = (x.TaxAmount.Amount + x.InsuranceAmount.Amount + x.OtherAmount.Amount) ?? 0,
                    Currency = x.TaxAmount.Currency.ToString(),
                }

            }).ToList();
        }
    }
}
