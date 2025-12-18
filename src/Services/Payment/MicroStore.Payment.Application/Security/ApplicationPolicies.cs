namespace MicroStore.Payment.Application.Security
{
    public class ApplicationPolicies
    {
        public const string RequireAuthenticatedUser = "ReuireAuthenticatedUser";

        public const string RequirePaymentWriteScope = "RequirePaymentWriteScope";

        public const string RequirePaymentReadScope = "RequirePaymentReadScope";
    }
}
