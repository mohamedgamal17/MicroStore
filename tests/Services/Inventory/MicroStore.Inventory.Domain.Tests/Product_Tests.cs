using FluentAssertions;
using MicroStore.Inventory.Domain.Events;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Domain.Tests
{
    public class ProductTests
    {

        [Test]
        public void Should_adjust_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0);

            product.AdjustInventory(15, "fake reason");
         
            product.Stock.Should().Be(15);

        }


        [Test]
        public void Should_allocate_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.AllocateStock(5);

            product.AllocatedStock.Should().Be(5);
            product.Stock.Should().Be(0);
           
        }


        [Test]
        public void Should_throw_invalid_operation_exception_when_allocated_stock_greater_than_product_stock()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            Action action = () =>  product.AllocateStock(10);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void Should_ship_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.AllocateStock(5);

            product.ShipProduct(5);

            product.AllocatedStock.Should().Be(0);
            product.Stock.Should().Be(0);
        }

        [Test]
        public void Should_throw_invalid_operation_exception_when_shipped_quantity_is_greater_than_allocated_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.AllocateStock(5);

            Action action = () => product.ShipProduct(10);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void Should_release_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.AllocateStock(5);

            product.ReleaseStock(5);

            product.AllocatedStock.Should().Be(0);
            product.Stock.Should().Be(5);
        }

        [Test]
        public void Should_throw_invalid_domain_exception_when_released_quantity_is_greater_than_allocated_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.AllocateStock(5);

            Action action = ()=> product.ReleaseStock(10);

            action.Should().ThrowExactly<InvalidOperationException>();
        }


        [Test]
        public void Should_recive_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0);

            product.ReciveQuantity(5);

            product.Stock.Should().Be(5);
        }

        [Test]
        public void Should_return_product_quantity()
        {
            Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 5);

            product.ReturnQuantity(5);

            product.Stock.Should().Be(10);

        }



    }
}