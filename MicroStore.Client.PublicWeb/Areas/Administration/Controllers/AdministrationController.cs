using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Infrastructure;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Area(AreaNames.Administration)]
    public  abstract class AdministrationController : AbpController
    {
        public UINotificationManager NotificationManager => LazyServiceProvider.LazyGetRequiredService<UINotificationManager>();

    }
}
