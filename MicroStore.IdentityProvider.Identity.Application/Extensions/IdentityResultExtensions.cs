using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.IdentityProvider.Identity.Application.Extensions
{
    public static class IdentityResultExtensions
    {

        public static ErrorInfo SerializeIdentityResultErrors(this IdentityResult identityResult)
        {
            if (identityResult.Succeeded)
            {
                throw new InvalidOperationException("identity result dose not contain any error to serialize");
            }

            return new ErrorInfo
            {
                Message = "Error while creating role see validation error for more details",
                ValidationErrors = identityResult.Errors.Select(x => new ValidationErrorInfo
                {
                    Message = x.Description
                }).ToArray()
            };
        }
    }
}
