using Microsoft.AspNetCore.Http;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class HttpContextExtensions
    {
        const string CDN_URL = "api/cdn";

        public static string GetHostUrl(this HttpContext context) 
        {
            return string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host);
        }

        public static string GetCurrentUrl(this HttpContext context)
        {
            return string.Format("{0}://{1}{2}", context.Request.Scheme, context.Request.Host,context.Request.PathBase);
        }

        public static string GenerateFileLink(this HttpContext contex, string filename)
        {
            return string.Format("{0}/{1}/{2}", contex.GetHostUrl() ,CDN_URL, filename);
        } 
    }
}
