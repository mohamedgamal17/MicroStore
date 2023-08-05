using Microsoft.AspNetCore.Identity;
using MicroStore.BuildingBlocks.Results;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace MicroStore.IdentityProvider.Identity.Application.Extensions
{
    public static class IdentityResultExtensions
    {
        public static void ThorwIfInvalidResult(this IdentityResult result)
        {
            if (result.Succeeded)
            {
                return;
            }


            throw new InvalidOperationException(result.Errors.Select(x => x.Description).JoinAsString(", "));
        }


        public static Result<T> ConvertToResult<T>(this IdentityResult result)
        {
            if (result.Succeeded)
            {
                return default(T);
            }

            var validationResult = result.Errors.Select(x => new ValidationResult( x.Description)).ToList();

            return new Result<T>(new AbpValidationException(validationResult));
        }
    }
}
