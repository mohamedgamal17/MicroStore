﻿using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Http;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Controllers
{
    [Area("BackEnd")]
    public class BackEndController : AbpController
    {
        protected ILogger<BackEndController> Logger => LazyServiceProvider.LazyGetRequiredService<ILogger<BackEndController>>();


        [NonAction]
        protected IActionResult HandleFailureResultWithView<T>(Result<T> result, object model = null)
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


        [NonAction]
        protected IActionResult HandleFailureResultWithJson<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.IsSuccess)
                {
                    throw new InvalidOperationException();
                }
            }

            Logger.LogException(result.Exception);



            if (result.Exception is EntityNotFoundException)
            {

                return StatusCode(StatusCodes.Status404NotFound, new RemoteServiceErrorInfo
                {
                    Message = result.Exception.Message,
                });

            }
            else if (result.Exception is BusinessException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new RemoteServiceErrorInfo
                {
                    Message = result.Exception.Message,
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new RemoteServiceErrorInfo
                {
                    Message = result.Exception.Message
                });
            }

        }

        public override JsonResult Json(object data)
        {
            return base.Json(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }
}