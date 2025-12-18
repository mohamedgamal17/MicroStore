using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;
using MicroStore.Inventory.Domain.Security;

namespace MicroStore.Inventory.Api.Controllers
{
    [ApiController]
    [Route("api/inventory/products")]
    public class InventoryItemController : MicroStoreApiController
    {
        private readonly IProductCommandService _productCommandService;

        private readonly IProductQueryService _productQueryService;

        public InventoryItemController(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<ProductDto>))]

        public async Task<IActionResult> RetriveProductList([FromQuery] PagingQueryParams query)
        {

            var result = await _productQueryService.ListAsync(query);

            return result.ToOk();

        }


        [HttpGet]
        [Route("sku/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<IActionResult> RetriveProductWithSku(string sku)
        {
  
            var result = await _productQueryService.GetBySkyAsync(sku);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
 
        public async Task<IActionResult> RetriveProductWithExternalId(string productId)
        {
            var result = await _productQueryService.GetAsync(productId);

            return result.ToOk();
        }

        [HttpPut]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
        public async Task<IActionResult> UpdateProductInventory(string productId, [FromBody] InventoryItemModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productCommandService.UpdateProductAsync(productId, model);

            return result.ToOk();
        }

        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(PagedResult<ProductDto>))]
        public async Task<IActionResult> Search(ProductSearchModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                return InvalideModelState();
            }

            var result = await _productQueryService.Search(model);

            return result.ToOk();
        }
    }
}
