using Microsoft.AspNetCore.Identity;

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
    }
}
