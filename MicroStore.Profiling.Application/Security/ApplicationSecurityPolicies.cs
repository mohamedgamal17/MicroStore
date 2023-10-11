namespace MicroStore.Profiling.Application.Security
{
    public class ApplicationSecurityPolicies
    {
        public const string RequireAuthenticatedUser = nameof(RequireAuthenticatedUser);

        public const string RequireProfilingReadScope = nameof(RequireProfilingReadScope);

        public const string RequireProfilingWriteScope = nameof(RequireProfilingWriteScope);

    }
}
