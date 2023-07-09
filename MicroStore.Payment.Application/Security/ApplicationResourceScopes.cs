namespace MicroStore.Payment.Application.Security
{
    public static class ApplicationResourceScopes
    {
        public const string Access = "billing.access";

        public static class Payment
        {
            public const string Read = "billing.read";

            public const string Write = "billing.write";
        }
    }
}
