using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.ShoppingGateway.ClinetSdk;
using NUglify.Helpers;

namespace MicroStore.Client.PublicWeb.Extensions
{
    public static class MicroStoreClientErrorExtensions
    {

        public static void MapToModelState(this MicroStoreError error , ModelStateDictionary modelState)
        {
            if(error != null)
            {
                //if (error.Title != null) modelState.AddModelError("", error.Title);
                //if (error.Type != null) modelState.AddModelError("", error.Type);
                //if (error.Detail != null) modelState.AddModelError("", error.Detail);
                if (error.Errors != null)
                {
                    error.Errors.ForEach(error => modelState.AddModelError("", string.Format("{0} : {1}", error.Key, error.Value.JoinAsString(" , "))));
                }


            }
        }
    }
}
