

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class AddressValidationResultModel
    {
        public bool IsValid { get; set; }
        public List<AddressValidationMessages> Messages { get; set; }

    }


    public class AddressValidationMessages
    {
        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

    }
}
