using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class FluentValidationResultExtensions
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
