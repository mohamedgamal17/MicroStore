namespace MicroStore.Payment.Domain.Security
{
    public static class BillingScope
    {
        public static class Payment
        {
            public static readonly string List = "billing.payment.list";

            public static readonly string Read = "billing.payment.read";

            public static readonly string Create = "billing.payment.create";

            public static readonly string Process = "billing.payment.process";

            public static readonly string Complete = "billing.payment.complete";

            public static readonly string Cancel = "billing.payment.cancel";
        }

        public static class System
        {
            public static readonly string List = "billing.system.list";

            public static readonly string Read = "billing.system.read";

            public static readonly string Update = "billing.system.update";
        }
    }
}
