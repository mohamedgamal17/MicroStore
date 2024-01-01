using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Products
{
    public class ProductCommandService : InventoryApplicationService, IProductCommandService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductCommandService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Result<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            Product product = new Product(model.ProductId);

            PrepareProductEntity(product,model);

            await _productRepository.InsertAsync(product);

            return ObjectMapper.Map<Product,ProductDto>(product);
        }

        public async Task<Result<ProductDto>> UpdateProductInfoAsync( ProductModel model, CancellationToken cancellationToken = default)
        {
            Product product = await _productRepository.SingleAsync(x => x.Id == model.ProductId);

            PrepareProductEntity(product,model);

            await _productRepository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<Result<ProductDto>> UpdateProductAsync(string id ,InventoryItemModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(ProductDto), id));
            }

            product.UpdateInventory(model.Stock);


            await _productRepository.UpdateAsync(product);


            return  ObjectMapper.Map<Product, ProductDto>(product);
        }

  
        private void PrepareProductEntity(Product product , ProductModel model)
        {
            product.Name = model.Name;
            product.Sku= model.Sku;
            product.Thumbnail = model.Thumbnail;
        }
    }
}
