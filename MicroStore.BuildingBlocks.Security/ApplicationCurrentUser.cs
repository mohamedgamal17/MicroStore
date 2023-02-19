using IdentityModel;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace MicroStore.BuildingBlocks.Security
{
   
    internal class ApplicationCurrentUser : IApplicationCurrentUser , ITransientDependency 
    {
        public string Id
        {
            get 
            {
                if (!IsAuthenticated)
                {
                    ThrowIfNotAuthorized();
                }

                return _userId!;
            }
        }
        private string? _userId => TryToGetUserId();

        public bool IsAuthenticated => TryToGetUserId() !=  null  ;

        public string UserName
        {
            get
            {
                if(!IsAuthenticated)
                {
                    ThrowIfNotAuthorized();
                }

               return  FindClaimValue(JwtClaimTypes.Name)!;
            }
        }

        public string Name
        {
            get
            {
                if (!IsAuthenticated)
                {
                    ThrowIfNotAuthorized();
                }

                return FindClaimValue(JwtClaimTypes.Name)!;
            }
        }

        public string SurName
        {
            get
            {
                if (!IsAuthenticated)
                {
                    ThrowIfNotAuthorized();
                }
                return FindClaimValue(JwtClaimTypes.FamilyName)!;
            }
        }

        public string? PhoneNumber
        {
            get
            {
                if (!IsAuthenticated)
                {
                    ThrowIfNotAuthorized();
                }

                return FindClaimValue(JwtClaimTypes.PhoneNumber);
            }
        }

        public bool PhoneNumberVerified => string.Equals(FindClaimValue(JwtClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public string? Email => FindClaimValue(JwtClaimTypes.Email);

        public bool EmailVerified => string.Equals(FindClaimValue(JwtClaimTypes.EmailVerified), "true", StringComparison.InvariantCultureIgnoreCase);


        public string[] Roles => FindClaims(JwtClaimTypes.Role).Select(x => x.Value).ToArray();


        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public ApplicationCurrentUser(ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public Claim? FindClaim(string claimType)
        {
            return _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == claimType);
        }

        public Claim[] FindClaims(string claimType)
        {
            return _currentPrincipalAccessor.Principal.Claims.Where(x => x.Type == claimType).ToArray();
        }

        public string? FindClaimValue(string claimType)
        {
            return FindClaim(claimType)?.Value;
        }



        public Claim[] GetAllClaims()
        {
            return _currentPrincipalAccessor.Principal.Claims.ToArray();
        }

        public bool IsInRole(string roleName)
        {
            return Roles.Any(x => x == roleName);
        }


        private string? TryToGetUserId() => _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value;



        private void ThrowIfNotAuthorized() => throw new InvalidOperationException("Current user is not authorized");
    }
}

