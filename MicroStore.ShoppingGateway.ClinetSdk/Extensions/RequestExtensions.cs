using MicroStore.ShoppingGateway.ClinetSdk.Services;

namespace MicroStore.ShoppingGateway.ClinetSdk.Extensions
{
    public static class RequestExtensions
    {
        const string Perfix = "param";

        public static Dictionary<string, string> ConvertToDictionary(this IQueryRequestOptions request)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            var requestType = request.GetType();

            requestType.GetProperties().ToList()
                 .ForEach((p) =>
                 {
                     dictionary.Add(string.Format("{0}.{1}", Perfix, p.Name), p.GetValue(request)?.ToString());
                 });


            return dictionary;
        }
    }
}
