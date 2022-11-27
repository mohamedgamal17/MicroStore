using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace MicroStore.Ordering.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [RemoteService(Name = "Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {


    }
}
