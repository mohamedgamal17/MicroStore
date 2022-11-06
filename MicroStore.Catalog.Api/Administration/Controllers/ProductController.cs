using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Api.Administration.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using System.Xml.Linq;
using Volo.Abp;

namespace MicroStore.Catalog.Api.Administration.Controllers
{
    [RemoteService(Name = "Product")]
    [Area("Administration")]
    [Route("api/[Area]/[Controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly ILocalMessageBus _localMessageBus;

        public ProductController(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }





        [Route("")]
        [HttpPost]
        public async Task<ProductDto> Post([FromBody] CreateProductModel model)
        {
            CreateProductCommand command = new CreateProductCommand
            {
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
            };


            var result = await _localMessageBus.Send(command);

            return result;
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<ProductDto> Put(Guid id, UpdateProductModel model)
        {

            UpdateProductCommand command = new UpdateProductCommand
            {
                ProductId = id,
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription
            };

            var result = await _localMessageBus.Send(command);

            return result;
        }



    }

}
