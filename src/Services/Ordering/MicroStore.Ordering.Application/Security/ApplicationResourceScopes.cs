namespace MicroStore.Ordering.Application.Security
{
    public class ApplicationResourceScopes
    {
        public const string Access = "ordering.access";
        public static class Order
        {
            public const string Write = "ordering.write";

            public const string Read = "ordering.read";
        }
    }
}
