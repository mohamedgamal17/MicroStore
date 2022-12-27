namespace MicroStore.Payment.Application.Abstractions.Consts
{
    public class PaymentMethodErrorType
    {
        public static string NotExist => "not_exist";
        public static string BusinessLogicError => "business_logic_error";
        public static string ValidationError => "validation_error";
    }
}
