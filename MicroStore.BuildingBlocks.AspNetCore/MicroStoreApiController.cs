using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.AspNetCore.Mvc;

namespace MicroStore.BuildingBlocks.AspNetCore
{
    public class MicroStoreApiController : AbpControllerBase
    {
        public ILocalMessageBus LocalMessageBus { get; set; } 

        public IActionResult FromResult(ResponseResult result)
        {
            return StatusCode(result.StatusCode, result.Envelope);
        }

        protected Task<ResponseResult> Send<TRequest>(TRequest request)
            where TRequest : IRequest<ResponseResult>
        {
            return LocalMessageBus.Send(request);
        }

    }
}