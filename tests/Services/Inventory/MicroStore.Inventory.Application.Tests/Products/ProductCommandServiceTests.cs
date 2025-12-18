using FluentAssertions;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;
using MicroStore.Inventory.Domain.ProductAggregate;
namespace MicroStore.Inventory.Application.Tests.Products
{
    public class ProductCommandServiceTests : BaseTestFixture
    {
        private readonly IProductCommandService _productCommandService;

        public ProductCommandServiceTests()
        {
            _productCommandService = GetRequiredService<IProductCommandService>();
        }

        [Test]
        public async Task Should_adjust_product_inventory()
        {

            //Product fakeProduct = await Insert(new Product() { Name = Guid.NewGuid().ToString(),Sku = Guid.NewGuid().ToString(), Thumbnail = Guid.NewGuid().ToString()});


            string productId = Guid.NewGuid().ToString();

            var result = await _productCommandService.CreateOrUpdateAsync(productId, new InventoryItemModel { Stock = 10 });

    
            result.IsSuccess.Should().BeTrue();


            Product product = await SingleAsync<Product>(x => x.Id == productId);

            product.Stock.Should().Be(10);

        }

        //[Test]
        //public async Task Should_return_failure_result_while_adjusting_product_quantity_when_product_is_not_exist()
        //{
        //    var result = await _productCommandService.CreateOrUpdateAsync(Guid.NewGuid().ToString(), new InventoryItemModel { Stock = 10 });

        //    result.IsFailure.Should().BeTrue();

        //    result.Exception.Should().BeOfType<EntityNotFoundException>();
        //}

    }
}
