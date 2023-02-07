namespace MicroStore.Payment.Domain.Shared.Security
{
    public static class BillingScope
    {

        public static List<string> List()
        {
            return new List<string>
            {
                Payment.List,
                Payment.Read,
                Payment.Create,
                Payment.Process,
                Payment.Complete,
            };
        }

        public static class Payment
        {
            public const string List = "billing.payment.list";

            public const string Read = "billing.payment.read";

            public const string Create = "billing.payment.create";

            public const string Process = "billing.payment.process";

            public const string Complete = "billing.payment.complete";

        }

    }
}
