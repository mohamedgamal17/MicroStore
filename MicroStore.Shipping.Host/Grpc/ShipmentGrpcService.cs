using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Shipments;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Shipping.Host.Grpc
{
    public class ShipmentGrpcService : ShipmentService.ShipmentServiceBase
    {
        private readonly IShipmentCommandService _shipmentCommandService;
        private readonly IShipmentQueryService _shipmentQueryService;
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
        public ShipmentGrpcService(IShipmentCommandService shipmentCommandService, IShipmentQueryService shipmentQueryService, IAbpLazyServiceProvider lazyServiceProvider)
        {
            _shipmentCommandService = shipmentCommandService;
            _shipmentQueryService = shipmentQueryService;
            LazyServiceProvider = lazyServiceProvider;
        }




        public override async Task<ShipmentResponse> Create(CreateShipmentReqeust request, ServerCallContext context)
        {
            var model = PrepareShipmentModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _shipmentCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareShipmentResponse(result.Value);
        }

        public override async Task<ShipmentResponse> Fullfill(FullfillRequest request, ServerCallContext context)
        {
            var model = PreparePacakgeModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _shipmentCommandService.FullfillAsync(request.ShipmentId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return PrepareShipmentResponse(result.Value);
        }
        public override async Task<ShipmentResponse> BuyLabel(BuyShipmentLabelRequest request, ServerCallContext context)
        {
            var model = new BuyShipmentLabelModel
            {
                ShipmentRateId = request.ShipmentRateId
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _shipmentCommandService.BuyLabelAsync(request.ShipmentId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return PrepareShipmentResponse(result.Value);
        }

        public override async Task<ShipmentRateListResponse> GetRates(GetShipmentRateReqeust request, ServerCallContext context)
        {
            var result = await _shipmentCommandService.RetriveShipmentRatesAsync(request.ShipmentId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return PrepareShipmentRateListResponse(result.Value);
        }

        public override async Task<ShipmentListResponse> GetList(ShipmentListRequest request, ServerCallContext context)
        {
            var model = PrepareShipmentListQueryModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _shipmentQueryService.ListAsync(model, request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareShipmentListResponse(result.Value);
        }

        public override async Task<ShipmentListByOrderResponse> GetListByOrderIds(ShipmentListByOrderIdsRequest request, ServerCallContext context)
        {
            var result = await _shipmentQueryService.ListByOrderIds(request.OrderIds.ToList());

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            var response = new ShipmentListByOrderResponse();

            foreach (var item in result.Value)
            {
                response.Items.Add(PrepareShipmentResponse(item));
            }

            return response;
        }

        public override async Task<ShipmentListByOrderResponse> GetListByOrderNumbers(ShipmentListByOrderNumbersRequest request, ServerCallContext context)
        {
            var result = await _shipmentQueryService.ListByOrderNumbers(request.OrderNumbers.ToList());

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            var response = new ShipmentListByOrderResponse();

            foreach (var item in result.Value)
            {
                response.Items.Add(PrepareShipmentResponse(item));
            }
            return response;
        }
        public override async Task<ShipmentResponse> GetById(GetShipmentById request, ServerCallContext context)
        {
            var result = await _shipmentQueryService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareShipmentResponse(result.Value);
        }

        public override async Task<ShipmentResponse> GetByOrderId(GetShipmentByOrderIdRequest request, ServerCallContext context)
        {
            var result = await _shipmentQueryService.GetByOrderIdAsync(request.OrderId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareShipmentResponse(result.Value);
        }

        public override async Task<ShipmentResponse> GetByOrderNumber(GetShipmentByOrderNumberRequest request, ServerCallContext context)
        {
            var result = await _shipmentQueryService.GetByOrderNumberAsync(request.OrderNumber);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareShipmentResponse(result.Value);
        }
        private ShipmentModel PrepareShipmentModel(CreateShipmentReqeust request)
        {
            var model = new ShipmentModel
            {
                OrderId = request.OrderId,
                OrderNumber = request.OrderNumber,
                UserId = request.UserId,
                Address = PrepareAddressReqeust(request.Address),
                Items = request.Items.Select(x => new ShipmentItemModel
                {
                    Name = x.Name,
                    Sku = x.Sku,
                    ProductId = x.ProductId,
                    Thumbnail = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = (decimal) x.UnitPrice,
                    Dimension = new DimensionModel
                    {
                        Length =x.Dimension.Length,
                        Height = x.Dimension.Height,
                        Width = x.Dimension.Width,
                        Unit = x.Dimension.Unit.ToString()
                    },
                    Weight = new WeightModel
                    {
                        Value = x.Weight.Value,
                        Unit = x.Weight.Unit.ToString()
                    }
                }).ToList()

            };

            return model;
        }

        private ShipmentListQueryModel PrepareShipmentListQueryModel(ShipmentListRequest request)
        {
            var model = new ShipmentListQueryModel
            {
                OrderNumber = request.OrderNumber,
                TrackingNumber = request.TrackingNumber,
                Status = request.Status,
                Country = request.Country,
                Skip = request.Skip,
                SortBy = request.SortBy,
                Length = request.Length,
                Desc = request.Desc
            };

            if (request.StartDate != null)
            {
                model.StartDate = request.StartDate.ToDateTime();
            }

            if (request.EndDate != null)
            {
                model.EndDate = request.EndDate.ToDateTime();
            }

            return model;

        }
        private PackageModel PreparePacakgeModel(FullfillRequest request)
        {
            var model = new PackageModel
            {
                Dimension = new DimensionModel
                {
                    Length = request.Dimension.Length,
                    Height = request.Dimension.Height,
                    Width = request.Dimension.Width,
                    Unit = request.Dimension.Unit.ToString()

                },
                Weight = new WeightModel
                {
                    Value = request.Weight.Value,
                    Unit = request.Weight.Unit.ToString()
                }
            };

            return model;
        }

        private ShipmentListResponse PrepareShipmentListResponse(PagedResult<ShipmentDto> paged)
        {
            var response = new ShipmentListResponse()
            {
                Length = paged.Lenght,
                Skip = paged.Skip,
                TotalCount = paged.TotalCount
            };

            foreach (var item in paged.Items)
            {
                response.Items.Add(PrepareShipmentResponse(item));
            }

            return response;
        }

        private ShipmentResponse PrepareShipmentResponse(ShipmentDto shipment)
        {
            var response = new ShipmentResponse
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                OrderNumber = shipment.OrderNumber,
                ShipmentExternalId = shipment.ShipmentExternalId,
                ShipmentLabelExternalId = shipment.ShipmentLabelExternalId,
                TrackingNumber = shipment.TrackingNumber,
                UserId = shipment.UserId,
                SystemName = shipment.SystemName,
                Status = System.Enum.Parse<ShipmentStatus>(shipment.Status),
                Address = new AddressResposne
                {
                    Name = shipment.Address.Name,
                    CountryCode = shipment.Address.CountryCode,
                    City = shipment.Address.City,
                    StateProvince = shipment.Address.State,
                    AddressLine1 = shipment.Address.AddressLine1,
                    AddressLine2 = shipment.Address.AddressLine2,
                    PostalCode = shipment.Address.PostalCode,
                    Phone = shipment.Address.Phone,
                    Zip = shipment.Address.Zip
                },
                CreatedAt = shipment.CreationTime.ToUniversalTime().ToTimestamp(),
                ModifiedAt = shipment.LastModificationTime?.ToUniversalTime().ToTimestamp()
            };

            if(shipment.Items != null)
            {
                foreach (var item in shipment.Items)
                {
                    var itemResponse = new ShipmentItemResponse
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Sku = item.Sku,
                        ProductId = item.ProductId,
                        Thumbnail = item.Thumbnail,
                        Quantity = item.Quantity,
                        UnitPrice = (double)item.UnitPrice,
                        Dimension = new Dimension
                        {
                            Length = item.Dimension.Lenght,
                            Height = item.Dimension.Height,
                            Width = item.Dimension.Width,
                            Unit = System.Enum.Parse<DimensionUnit>(item.Dimension.Unit),
                        },
                        Weight = new Weight
                        {
                            Value = item.Weight.Value,
                            Unit = System.Enum.Parse<WeightUnit>(item.Weight.Unit,true)
                        }
                    };

                    response.Items.Add(itemResponse);
                }
            }
            

            return response;
        }


        private ShipmentRateListResponse PrepareShipmentRateListResponse(List<ShipmentRateDto> items)
        {
            var response = new ShipmentRateListResponse();

            foreach (var item in items)
            {
                response.Items.Add(PrepareShipmentRateResponse(item));
            }

            return response;
        }
        private ShipmentRateResponse PrepareShipmentRateResponse(ShipmentRateDto shipmentRate)
        {
            return new ShipmentRateResponse
            {
                CarrierId = shipmentRate.CarrierId,
                Amount = new MoneyResponse
                {
                    Value = shipmentRate.Amount?.Value ?? 0,
                    Currency = shipmentRate.Amount?.Currency ?? string.Empty
                },
                ServiceLevel = new ServiceLevelResponse
                {
                    Name = shipmentRate.ServiceLevel?.Name,
                    Code = shipmentRate.ServiceLevel?.Code
                },
                Days = shipmentRate.Days ?? 0
            };
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
