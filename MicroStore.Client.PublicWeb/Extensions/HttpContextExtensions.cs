namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class HttpContextExtensions
    {

        public static string GetHostUrl(this HttpContext context) 
        {
            return string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host);
        }

        public static string GetCurrentUrl(this HttpContext context)
        {
            return string.Format("{0}://{1}{2}", context.Request.Scheme, context.Request.Host,context.Request.PathBase);
        }
    }
}
