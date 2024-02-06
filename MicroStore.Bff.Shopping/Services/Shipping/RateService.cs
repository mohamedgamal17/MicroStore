using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Grpc.Shipping;
using MicroStore.Bff.Shopping.Models.Shipping;
namespace MicroStore.Bff.Shopping.Services.Shipping
{
    public class RateService
    {
        private readonly Grpc.Shipping.RateService.RateServiceClient _rateServiceClient;

        public RateService(Grpc.Shipping.RateService.RateServiceClient rateServiceClient)
        {
            _rateServiceClient = rateServiceClient;
        }


        public async Task<List<Rate>> EstimateAsync(EstimateRateModel model , CancellationToken cancellationToken = default)
        {
            var request = PrepareEstimateShippingRequest(model);

            var response = await _rateServiceClient.EstimateAsync(request);

            var result = response.Items.Select(PrepareRate).ToList();

            return result;
        }
        public EstimateShippingRateRequest PrepareEstimateShippingRequest(EstimateRateModel model)
        {
            var request = new EstimateShippingRateRequest
            {
                Address = new AddressRequest
                {
                    Name = model.Address.Name,
                    CountryCode = model.Address.Country,
                    StateProvince = model.Address.State,
                    City = model.Address.City,
                    AddressLine1 = model.Address.AddressLine1,
                    AddressLine2 = model.Address.AddressLine2,
                    Phone = model.Address.Phone,
                    PostalCode = model.Address.PostalCode,
                    Zip = model.Address.Zip
                },

            };

            foreach(var item in model.Items)
            {
                var estimatedItem = new ShipmentItemEstimatedRequest
                {
                    Name = item.name,
                    Sku = item.sku,
                    Quantity = item.Quantity,
                    UnitPrice = new MoneyResponse
                    {
                        Currency = item.UnitPrice?.Currency ?? string.Empty,
                        Value = item.UnitPrice?.Value ?? -1
                    },
                    Weight = new Weight
                    {
                        Unit = (WeightUnit)item.Weight.Unit,
                        Value = item.Weight.Value
                    },

                };

                request.Items.Add(estimatedItem);
            }

            return request;
        }

        private Rate PrepareRate(RateResponse response)
        {
            var rate = new Rate
            {
                Name = response.Name,
                Money = new Data.Common.Money
                {
                    Value = response.Money?.Value ?? -1,
                    Currency = response.Money?.Currency ?? string.Empty
                },
                ShippingDate = response.ShippingDate?.ToDateTime(),
                EstimatedDays = response.EstimatedDays
            };

            return rate;
        }
    }
}
