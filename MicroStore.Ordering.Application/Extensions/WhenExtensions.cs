namespace MicroStore.Ordering.Application.Extensions
{
    public static class WhenExtensions
    {
        public static T When<T>(this T source, bool condition, Func<T, T> func)
        {
            if (condition)
                return func(source);

            return source;
        }

        public static void When<T>(this T source, bool condition, Action<T> action)
        {
            if (condition)
                action(source);
            return;
        }
    }
}
