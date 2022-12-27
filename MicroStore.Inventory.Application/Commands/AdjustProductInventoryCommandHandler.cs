using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Commands
{
    public class AdjustProductInventoryCommandHandler : CommandHandlerV1<AdjustProductInventoryCommand>
    {

        private readonly IRepository<Product> _productRepository;

        public AdjustProductInventoryCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ResponseResult> Handle(AdjustProductInventoryCommand request, CancellationToken cancellationToken)
        {
           Product? product  = await  _productRepository.SingleOrDefaultAsync(x => x.Sku == request.Sku);

            if(product == null)
            {
                var erroInfo = new ErrorInfo
                {
                    Message = $"Product with sku : {request.Sku} is not exist"
                };

                return ResponseResult.Failure((int)HttpStatusCode.NotFound, erroInfo);
            }

            product.AdjustInventory(request.Stock, request.Reason);


            await _productRepository.UpdateAsync(product);

            var result = new ProductAdjustedInventoryDto
            {
                ProductId = product.Id,
                ExternalProductId = product.ExternalProductId,
                Name = product.Name,
                Sku = product.Sku,
                Thumbnail = product.Thumbnail,
                AdjustedStock = product.Stock
            };

            return ResponseResult.Success((int) HttpStatusCode.Accepted, result);
        }
    }
}
