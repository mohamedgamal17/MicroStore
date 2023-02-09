namespace MicroStore.Gateway.Shopping.Security
{
    public static class BillingScope
    {

        public static List<string> List()
        {
            return new List<string>
            {
                Payment.Read,
                Payment.Write,
            };
        }

        public static class Payment
        {

            public const string Read = "billing.payment.read";

            public const string Write = "billing.payment.write";

        }       
    }
}
