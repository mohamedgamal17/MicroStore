using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Commands
{
    public class DispatchProductCommandHandler : CommandHandlerV1<DispatchProductCommand>
    {
        private readonly IRepository<Product> _productRepository;

        public DispatchProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ResponseResult> Handle(DispatchProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product(
                request.ExternalProductId,
                request.Name,
                request.Sku,
                request.Thumbnail, 0);

            await _productRepository.InsertAsync(product);

            return ResponseResult.Success((int) HttpStatusCode.Created);
        }
    }
}
