using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Controller
{
    [Area("BackEnd")]
    public class BackEndController : AbpController
    {
        protected  ILogger<BackEndController> Logger => LazyServiceProvider.LazyGetRequiredService<ILogger<BackEndController>>();

      
        [NonAction]
        protected IActionResult HandleFailureResult<T>(Result<T> result, object model = null)
        {

            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            Logger.LogException(result.Exception);

            if (result.Exception is EntityNotFoundException)
            {
                return NotFound();

            }
            else if (result.Exception is BusinessException)
            {
                ModelState.AddModelError("", result.Exception.Message);

                return View(model);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}
