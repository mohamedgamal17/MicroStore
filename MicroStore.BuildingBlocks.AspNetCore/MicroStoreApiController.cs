using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.BuildingBlocks.AspNetCore
{
    public class MicroStoreApiController : AbpControllerBase
    {
        public ILocalMessageBus LocalMessageBus { get; set; }

        [NonAction]
        public IActionResult FromResult<T>(ResponseResult<T> result)
        {
            return StatusCode(result.StatusCode, result.EnvelopeResult);
        }

        [NonAction]
        protected Task<ResponseResult<TResponse>> Send<TResponse>(IRequest<TResponse> request)
        {
            return LocalMessageBus.Send(request);
        }

    }
}