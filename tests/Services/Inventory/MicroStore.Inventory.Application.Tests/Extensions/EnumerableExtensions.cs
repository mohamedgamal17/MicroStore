namespace MicroStore.Inventory.Application.Tests.Extensions
{
    public static class EnumerableExtensions
    {

        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (var item in list)
            {
                await func(item);
            }
        }
    }
}
