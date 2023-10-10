namespace MicroStore.Ordering.Application.Domain
{
    public static class OrderStatusConst
    {
        public static string Submited => nameof(Submited);
        public static string Accepted => nameof(Accepted);
        public static string Approved => nameof(Approved);
        public static string Fullfilled => nameof(Fullfilled);
        public static string Completed => nameof(Completed);
        public static string Cancelled => nameof(Cancelled);

    }
}
