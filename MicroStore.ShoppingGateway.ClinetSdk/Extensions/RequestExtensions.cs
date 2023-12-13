using MicroStore.ShoppingGateway.ClinetSdk.Services;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace MicroStore.ShoppingGateway.ClinetSdk.Extensions
{
    public static class RequestExtensions
    {

        public static Dictionary<string, string> ConvertToDictionary(this object request)
        {
            if(request == null)
            {
                return new Dictionary<string, string>();
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            var requestType = request.GetType();

            requestType.GetProperties().ToList()
                 .ForEach((p) =>
                 {
                     string value = string.Empty;

                     if(TryToGetPropertyValueValue(p, request, out var val))
                     {

                         if(val.GetType() == typeof(DateTime))
                         {
                             DateTime dateTime = (DateTime)val;

                             dictionary.Add(string.Format("{0}", ConvertToSnakeCase(p.Name)),  dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));

                         }
                         else
                         {
                             dictionary.Add(string.Format("{0}", ConvertToSnakeCase(p.Name)),val.ToString());
                         }

             
                     }

                 });


            return dictionary;
        }
        
        private static bool TryToGetPropertyValueValue(PropertyInfo propertyInfo , Object obj , out object val)
        {
            val = propertyInfo.GetValue(obj);

            if (val != null)
                return true;

            return false;
        }
        private static string ConvertToSnakeCase(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            var builder = new StringBuilder(str.Length + Math.Min(2, str.Length / 5));
            var previousCategory = default(UnicodeCategory?);

            for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
            {
                var currentChar = str[currentIndex];
                if (currentChar == '_')
                {
                    builder.Append('_');
                    previousCategory = null;
                    continue;
                }

                var currentCategory = char.GetUnicodeCategory(currentChar);
                switch (currentCategory)
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                        if (previousCategory == UnicodeCategory.SpaceSeparator ||
                            previousCategory == UnicodeCategory.LowercaseLetter ||
                            previousCategory != UnicodeCategory.DecimalDigitNumber &&
                            previousCategory != null &&
                            currentIndex > 0 &&
                            currentIndex + 1 < str.Length &&
                            char.IsLower(str[currentIndex + 1]))
                        {
                            builder.Append('_');
                        }

                        currentChar = char.ToLower(currentChar);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        if (previousCategory == UnicodeCategory.SpaceSeparator)
                        {
                            builder.Append('_');
                        }
                        break;

                    default:
                        if (previousCategory != null)
                        {
                            previousCategory = UnicodeCategory.SpaceSeparator;
                        }
                        continue;
                }

                builder.Append(currentChar);
                previousCategory = currentCategory;
            }

            return builder.ToString();
        }
    }
}
