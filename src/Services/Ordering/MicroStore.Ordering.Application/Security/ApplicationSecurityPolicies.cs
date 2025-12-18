namespace MicroStore.Ordering.Application.Security
{
    public class ApplicationSecurityPolicies
    {
        public const string RequireAuthenticatedUser = nameof(RequireAuthenticatedUser);

        public const string RequireOrderReadScope = nameof(RequireOrderReadScope);

        public const string RequireOrderWriteScope = nameof(RequireOrderWriteScope);
    }
}
