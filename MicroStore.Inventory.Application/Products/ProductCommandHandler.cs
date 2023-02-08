using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Products
{
    public class ProductCommandHandler : RequestHandler,
        ICommandHandler<DispatchProductCommand>,
        ICommandHandler<UpdateProductCommand>,
        ICommandHandler<AdjustProductInventoryCommand,ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        public ProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseResult<Unit>> Handle(DispatchProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product(
             request.ExternalProductId,
             request.Name,
             request.Sku,
             request.Thumbnail, 0);

            await _productRepository.InsertAsync(product);

            return ResponseResult.Success((int)HttpStatusCode.Created);
        }

        public async Task<ResponseResult<Unit>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = await _productRepository.SingleAsync(x => x.ExternalProductId == request.ExternalProductId);

            product.Sku = request.Sku;
            product.Name = request.Name;
            product.Thumbnail = request.Thumbnail;

            await _productRepository.UpdateAsync(product);

            return ResponseResult.Success((int)HttpStatusCode.Created);
        }

        public async Task<ResponseResult<ProductDto>> Handle(AdjustProductInventoryCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Sku == request.Sku);

            if (product == null)
            {
                return Failure <ProductDto>(HttpStatusCode.NotFound, $"Product with sku : {request.Sku} is not exist");
            }

            product.AdjustInventory(request.Stock, request.Reason);


            await _productRepository.UpdateAsync(product);



            return Success(HttpStatusCode.OK, ObjectMapper.Map<Product, ProductDto>(product));
        }
    }
}
