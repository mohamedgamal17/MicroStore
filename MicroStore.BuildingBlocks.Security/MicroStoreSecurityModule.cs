using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;

namespace MicroStore.BuildingBlocks.Security
{
    [DependsOn(typeof(AbpSecurityModule),
        typeof(AbpAuthorizationModule))]
    public class MicroStoreSecurityModule : AbpModule
    {

    }
}