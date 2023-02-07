namespace MicroStore.Payment.Domain.Shared
{
    public static class PaymentMethodErrorType
    {
        public const string NotExist = "not_exist";

        public const string BusinessLogicError = "business_logic_error";

        public const string ValidationError = "validation_error";

        public const string BadGateway = "bad_gateway";
    }
}
