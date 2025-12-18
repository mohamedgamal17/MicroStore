using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class TempDataExtensions
    {
        public static void Put(this ITempDataDictionary tempData, string key, object? value) 
        {
            if(value != null)
            {
                tempData[key] = JsonConvert.SerializeObject(value);
            }
          
        }

        public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
