﻿using Google.Protobuf.WellKnownTypes;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Common;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Grpc.Geographic;
using MicroStore.Bff.Shopping.Grpc.Shipping;
using MicroStore.Bff.Shopping.Models.Shipping;
namespace MicroStore.Bff.Shopping.Services.Shipping
{
    public class ShipmentService
    {
        private readonly Grpc.Shipping.ShipmentService.ShipmentServiceClient _shipmentServiceClient;

        private readonly Grpc.Geographic.CountryService.CountryServiceClient _countryServiceClient;

        public ShipmentService(Grpc.Shipping.ShipmentService.ShipmentServiceClient shipmentServiceClient, Grpc.Geographic.CountryService.CountryServiceClient countryServiceClient)
        {
            _shipmentServiceClient = shipmentServiceClient;
            _countryServiceClient = countryServiceClient;
        }

        public async Task<Shipment> CreateAsync(ShipmentModel model, CancellationToken cancellationToken = default)
        {
            var request = PrepareCreateShipmentRequest(model);

            var response = await _shipmentServiceClient.CreateAsync(request);

            return await PrepareShipment(response);
        }

        public async Task<Shipment> FullfillAsync(string shipmentId, FullfillModel model, CancellationToken cancellationToken = default)
        {
            var request = PrepareFullfillRequest(shipmentId, model);

            var response = await _shipmentServiceClient.FullfillAsync(request);

            return await PrepareShipment(response);
        }

        public async Task<Shipment> BuyLabelAsync(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default)
        {
            var request = new BuyShipmentLabelRequest
            {
                ShipmentId = shipmentId,
                ShipmentRateId = model.ShipmentRateId
            };

            var response = await _shipmentServiceClient.BuyLabelAsync(request);

            return await PrepareShipment(response);
        }

        public async Task<PagedList<Shipment>> ListAsync(string? userId =null,string? orderNumber = null,string? trackingNumber = null ,int status = -1, string? country= null, DateTime? startDate = null, DateTime? endDate = null, int skip = 0 , int length = 10 , string? sortBy = null, bool desc = false   ,CancellationToken cancellationToken = default)
        {
            var request = new ShipmentListRequest
            {
                UserId = userId,
                OrderNumber = orderNumber,
                TrackingNumber = trackingNumber,
                Status = status,
                Country = country,
                StartDate = startDate?.ToUniversalTime().ToTimestamp(),
                EndDate = startDate?.ToUniversalTime().ToTimestamp(),
                Skip = skip,
                Length = length,
                SortBy = sortBy,
                Desc = desc
            };

            var response = await _shipmentServiceClient.GetListAsync(request);

            
            var paged = new PagedList<Shipment>
            {
                Length = response.Length,
                Skip = response.Skip,
                TotalCount = response.TotalCount,
                Items = await Task.WhenAll(response.Items.Select(PrepareShipment))
            };

            return paged;
        }

        public async Task<Shipment> GetAsync(string shipmentId , CancellationToken cancellationToken = default)
        {
            var request = new GetShipmentById { Id = shipmentId };

            var response = await _shipmentServiceClient.GetByIdAsync(request);

            return await PrepareShipment(response);
        }

        public async Task<List<ShipmentRate>> RetriveShipmentRatesAsync(string shipmentId , CancellationToken cancellationToken = default)
        {
            var request = new GetShipmentRateReqeust { ShipmentId = shipmentId };

            var response = await _shipmentServiceClient.GetRatesAsync(request);

            var result = response.Items.Select(x => new ShipmentRate
            {
                Amount = new Money
                {
                    Currency = x.Amount?.Currency ?? string.Empty,
                    Value = x.Amount?.Value ?? -1
                },
                ServiceLevel = new ServiceLevel
                {
                    Code = x.ServiceLevel?.Code ?? string.Empty,
                    Name = x.ServiceLevel?.Name ?? string.Empty
                },
                Days = x.Days
            }).ToList();


            return result;
        }
        private CreateShipmentReqeust PrepareCreateShipmentRequest(ShipmentModel model)
        {
            var request = new CreateShipmentReqeust
            {
                OrderId = model.OrderId,
                OrderNumber = model.OrderNumber,
                UserId = model.UserId,
                Address = new AddressRequest
                {
                    Name = model.Address.Name,
                    CountryCode = model.Address.Country,
                    City = model.Address.City,
                    StateProvince = model.Address.State,
                    AddressLine1 = model.Address.AddressLine1,
                    AddressLine2 = model.Address.AddressLine2,
                    Phone = model.Address.Phone,
                    Zip = model.Address.Zip,
                    PostalCode = model.Address.PostalCode
                }
            };

            if(model.Items != null)
            {
                foreach (var item in model.Items)
                {
                    var shipmentItem = new ShipmentItemRequest
                    {
                        Name = item.Name,
                        Sku = item.Sku,
                        ProductId = item.ProductId,
                        Thumbnail = item.Thumbnail,
                        Dimension = new Grpc.Shipping.Dimension
                        {
                            Length = item.Dimension.Length,
                            Height = item.Dimension.Height,
                            Width = item.Dimension.Width,
                            Unit = (Grpc.Shipping.DimensionUnit)item.Dimension.Unit,

                        },
                        Weight = new Grpc.Shipping.Weight
                        {
                            Value = item.Weight.Value,
                            Unit = (Grpc.Shipping.WeightUnit)item.Weight.Unit
                        },
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };

                    request.Items.Add(shipmentItem);
                }
            }
           

            return request;
        }

        private FullfillRequest PrepareFullfillRequest( string shipmentId,FullfillModel model)
        {
            var request = new FullfillRequest
            {
                ShipmentId = shipmentId,
                Weight = new Grpc.Shipping.Weight
                {
                    Value = model.Weight.Value,
                    Unit = (Grpc.Shipping.WeightUnit)model.Weight.Unit
                },
                Dimension = new Grpc.Shipping.Dimension
                {
                    Length = model.Dimensions.Length,
                    Height = model.Dimensions.Height,
                    Width = model.Dimensions.Width,
                    Unit = (Grpc.Shipping.DimensionUnit)model.Dimensions.Unit
                }
            };

            return request;
        }
        private async Task<Shipment> PrepareShipment(ShipmentResponse response)
        {
            var address = await PrepareAddress(response.Address);

            var shipment = new Shipment
            {
                Id = response.Id,
                ShipmentExternalId = response.ShipmentExternalId,
                ShipmentLabelExternalId = response.ShipmentLabelExternalId,
                TrackingNumber = response.TrackingNumber,
                OrderId = response.OrderId,
                OrderNumber = response.OrderNumber,
                UserId = response.UserId,
                Status = (Data.Shipping.ShipmentStatus)response.Status,
                SystemName = response.SystemName,
                Address = address,
                Items = response.Items.Select(x => new ShipmentItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sku = x.Sku,
                    ProductId = x.ProductId,
                    Thumbnail = x.Thumbnail,
                    Dimension = new Data.Common.Dimension
                    {
                        Height = x.Dimension.Height,
                        Width = x.Dimension.Width,
                        Length = x.Dimension.Length,
                        Unit = (Data.Common.DimensionUnit) x.Dimension.Unit
                    },
                    Weight = new Data.Common.Weight
                    {
                        Value = x.Weight.Value,
                        Unit = (Data.Common.WeightUnit)x.Weight.Unit
                    },
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList(),
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()

            };

            return shipment;
        }

        private async Task<Address> PrepareAddress(AddressResposne response)
        {
            var request = new GetCountryByCodeRequest { Code = response.CountryCode };

            var country = await _countryServiceClient.GetByCodeAsync(request);

            var stateProvince = country.States.Single(x => x.Abbrevation == response.StateProvince);

            var address = new Address
            {
                Name = response.Name,
                Country = new AddressCountry
                {
                    Name = country.Name,
                    TwoIsoCode = country.TwoLetterIsoCode,
                    ThreeIsoCode = country.ThreeLetterIsoCode,
                    NumericIsoCode = country.NumericIsoCode
                },
                State = new AddressStateProvince
                {
                    Name = stateProvince.Name,
                    Abbreviation = stateProvince.Abbrevation
                },
                City = response.City,
                AddressLine1 = response.AddressLine1,
                AddressLine2 = response.AddressLine2,
                Phone = response.Phone,
                PostalCode = response.PostalCode,
                Zip = response.Zip

            };

            return address;
        }
    }
}
