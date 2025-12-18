using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Inventory.Application.Products
{
    public class ProductCommandService : InventoryApplicationService, IProductCommandService
    {

        private readonly IProductRepository _productRepository;

        public ProductCommandService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ProductDto>> CreateOrUpdateAsync(string id ,InventoryItemModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                product = new Product(id);

            }
      
            product.UpdateInventory(model.Stock);
         
            await _productRepository.AddOrUpdateAsync(product);

            return  ObjectMapper.Map<Product, ProductDto>(product);
        }
    }
}
