namespace MicroStore.Shipping.Domain.Security
{
    public class ApplicationPolicies
    {
        public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";

        public const string RequireShippingReadScope = "RequireShippingReadScope";
    }
}
