
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MicroStore.BuildingBlocks.AspNetCore
{
    public static class ModelStateExtensions
    {

        public static Dictionary<string, string[]> ConvertModelStateErrors(this ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            var errorDictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = errors[0].ErrorMessage;
                        errorDictionary.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = errors[i].ErrorMessage;
                        }

                        errorDictionary.Add(key, errorMessages);
                    }
                }
            }

            return errorDictionary;
        }
    }
}
