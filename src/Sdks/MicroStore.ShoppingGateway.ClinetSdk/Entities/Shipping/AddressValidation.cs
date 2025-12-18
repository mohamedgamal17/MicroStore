namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping
{
    public class AddressValidation
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
