using FluentAssertions;
using MicroStore.Inventory.Application.ProductAggregate;
using MicroStore.Inventory.Application.ProductAggregate.Exceptions;
using MicroStore.Inventory.Events;
using NUnit.Framework;

namespace MicroStore.Inventory.Application.Tests.ProductAggregate
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class ProductTests : AggregateRootTest<Product>
    {
        public ProductTests() 
            : base(new Product(Guid.NewGuid(), new CurrentState()))
        {

        }

        [Test]
        public void Should_raise_product_disaptched_event_when_dispatch_new_product()
        {
            string fakename = Guid.NewGuid().ToString();
            string fakeSku = Guid.NewGuid().ToString();

            When(product => product.Dispatch(fakename, fakeSku));

            Then<ProductDispatchedEvent>((evnt) =>
            {
                evnt.Sku.Should().Be(fakeSku);
                evnt.Name.Should().Be(fakename);
                evnt.EventType.Should().Be(nameof(ProductDispatchedEvent));
            });
        }

        [Test]
        public void Should_raise_inventory_adjusted_event_when_stock_adjusted()
        {

            When(pr => pr.AdJustInventory(5,"fake reason", DateTime.UtcNow));

            Then<InventoryAdjustedEvent>(evnt =>
            {
                evnt.AdjustedQuantity.Should().Be(5);

                evnt.EventType.Should().Be(nameof(InventoryAdjustedEvent));

            });
        }

        [Test]
        public void Should_throw_invalid_domain_exception_when_quantity_on_hand_is_negative()
        {

            Throws<InvalidDomainException>(pr => pr.AdJustInventory(-10,"fake reason",DateTime.UtcNow), ex =>
            {
                ex.Message.Should().Be("Cannot adjust product quantity");
            });
        }

        [Test]
        public void Should_raise_product_recived_event_when_recive_prdouct()
        {
            When(pr => pr.ReciveProduct(5, DateTime.UtcNow));

            Then<ProductRecivedEvent>(evnt =>
            {
                evnt.RecivedQuantity.Should().Be(5);
                evnt.EventType.Should().Be(nameof(ProductRecivedEvent));
            });

        }

        [Test]
        public void Should_raise_product_allocated_event_when_allocate_product_stock()
        {
            Given(new ProductRecivedEvent(5, DateTime.UtcNow));

            When(pr => pr.AllocateStock(5,DateTime.UtcNow));

            Then<ProductAllocatedEvent>(evnt =>
            {
                evnt.AllocatedQuantity.Should().Be(5);
                evnt.EventType.Should().Be(nameof(ProductAllocatedEvent));
            });
        }

        [Test]
        public void Should_throw_Invalid_domain_exception_when_allocated_stock_greater_than_product_stock()
        {

            Given(new ProductRecivedEvent(5, DateTime.UtcNow));

            Throws<InvalidDomainException>(pr => pr.AllocateStock(10,DateTime.UtcNow), ex =>
            {
                ex.Message.Should().Be("Product stock is less than requested allocate quantity");
            });
      
        }

        [Test]
        public void Should_raise_product_shipped_event_when_ship_product()
        {
        
            Given(
                    new ProductRecivedEvent(5, DateTime.UtcNow),
                    new ProductAllocatedEvent(5,DateTime.UtcNow)
                );

            When(pr => pr.ShipProduct(5, DateTime.UtcNow));

            Then<ProductShippedEvent>(evnt =>
            {
                evnt.ShippedQuantity.Should().Be(5);
                evnt.EventType.Should().Be(nameof(ProductShippedEvent));
            });
        }

        [Test]
        public void Should_throw_invalid_domain_exception_when_shipped_quantity_is_greater_than_allocated_quantity()
        {
            Given(
                    new ProductRecivedEvent(5, DateTime.UtcNow),
                    new ProductAllocatedEvent(5, DateTime.UtcNow)
                );

            Throws<InvalidDomainException>(pr => pr.ShipProduct(15,DateTime.UtcNow), ex =>
            {
                ex.Message.Should().Be("Allocated stock is less than requested ship quantity");
            });
        }

        [Test]
        public void Should_raise_product_released_event_when_release_product_stock()
        {
            Given(
                   new ProductRecivedEvent(5, DateTime.UtcNow),
                   new ProductAllocatedEvent(5, DateTime.UtcNow)
                );

            When(pr => pr.ReleaseStock(5, DateTime.UtcNow));

            Then<ProductReleasedEvent>(evnt =>
            {
                evnt.ReleasedQuantity.Should().Be(5);
                evnt.EventType.Should().Be(nameof(ProductReleasedEvent));
            });
        }

        [Test]
        public void Should_throws_invalid_domain_exception_when_released_quantity_is_greater_than_allocated_quantity()
        {
            Given(
                    new ProductRecivedEvent(5, DateTime.UtcNow),
                    new ProductAllocatedEvent(5, DateTime.UtcNow)
                );

            Throws<InvalidDomainException>(pr => pr.ReleaseStock(15,DateTime.UtcNow), ex =>
            {
                ex.Message.Should().Be("Allocated stock is les than requested release quantity");
            });
        }


        [Test]
        public void Should_raise_product_returned_event_when_return_product_stock()
        {
            When(pr => pr.ReturnStock(5, DateTime.UtcNow));

            Then<ProductReturnedEvent>(evnt =>
            {
                evnt.ReturnedQuantity.Should().Be(5);
                evnt.EventType.Should().Be(nameof(ProductReturnedEvent));
            });
        }

    }
}
