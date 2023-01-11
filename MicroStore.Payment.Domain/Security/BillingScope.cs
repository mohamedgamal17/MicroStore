namespace MicroStore.Payment.Domain.Security
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
                Payment.Cancel,
                System.List,
                System.Read,
                System.Update
            };
        }

        public static class Payment
        {
            public const string List = "billing.payment.list";

            public const string Read = "billing.payment.read";

            public const string Create = "billing.payment.create";

            public const string Process = "billing.payment.process";

            public const string Complete = "billing.payment.complete";

            public const string Cancel = "billing.payment.cancel";
        }

        public static class System
        {
            public const string List = "billing.system.list";

            public const string Read = "billing.system.read";

            public const string Update = "billing.system.update";
        }
    }
}
