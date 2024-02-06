
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Grpc.Shipping;
using MicroStore.Bff.Shopping.Models.Common;

namespace MicroStore.Bff.Shopping.Services.Shipping
{
    public class AddressService
    {

        private readonly Grpc.Shipping.AddressService.AddressServiceClient _addressServiceClient;

        public AddressService(Grpc.Shipping.AddressService.AddressServiceClient addressServiceClient)
        {
            _addressServiceClient = addressServiceClient;
        }

        public async Task<AddressValidationResult> ValidateAsync(AddressModel addressRequest)
        {
            var request = PrepareAddressRequest(addressRequest);

            var response = await _addressServiceClient.ValidateAsync(request);

            var result = new AddressValidationResult
            {
                IsValid = response.IsValid
            };

            if (!response.IsValid)
            {
                result.Messages = response.Messages.Select(x => new AddressValidationMessages
                {
                    Type = x.Type,
                    Message = x.Message,
                    Code = x.Code
                }).ToList();
            }

            return result;
        }


        private AddressRequest PrepareAddressRequest(AddressModel model)
        {
            return new AddressRequest
            {
                Name = model.Name,
                CountryCode = model.Country,
                City = model.City,
                StateProvince = model.State,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Phone = model.Phone,
                Zip = model.Zip,
                PostalCode = model.PostalCode,
               
            };
        }
    }
}
