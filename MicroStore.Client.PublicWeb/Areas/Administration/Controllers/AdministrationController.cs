using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Area(AreaNames.Administration)]
    public  abstract class AdministrationController : AbpController
    {
        public UINotificationManager NotificationManager => LazyServiceProvider.LazyGetRequiredService<UINotificationManager>();

    }
}
