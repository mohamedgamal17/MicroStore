namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Navigation
{
    public static class BackEndMenusStandard
    {
        public static class UserMenus
        {
            public const string Index = "MicroStore.Identity.User";

            public const string Create = "MicroStore.Identity.User.Create";

            public const string Edit = "MicroStore.Identity.User.Edit";
        }

        public static class RoleMenus
        {
            public const string Index = "MicroStore.Identity.Role";

            public const string Create = "MicroStore.Identity.Role.Create";

            public const string Edit = "MicroStore.Identity.Role.Edit";
        }

        public static class ApiResourceMenus
        {
            public const string Index = "MicroStore.IdentityServer.ApiResource";

            public const string Create = "MicroStore.IdentityServer.ApiResource.Create";

            public const string Edit = "MicroStore.IdentityServer.ApiResource.Edit";
        }    
        
        public static class ClientMenus
        {
            public const string Index = "MicroStore.IdentityServer.Client";

            public const string Create = "MicroStore.IdentityServer.Client.Create";

            public const string Edit = "MicroStore.IdentityServer.Client.Edit";
        }

        public static class ApiScopeMenus
        {
            public const string Index = "MicroStore.IdentityServer.ApiScope";

            public const string Create = "MicroStore.IdentityServer.ApiScope.Create";

            public const string Edit = "MicroStore.IdentityServer.ApiScope.Edit";
        }
    }
}
