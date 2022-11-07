using FluentAssertions;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Domain.Tests
{
    public class BasketTests
    {

        [Test]
        public void ShouldAddNewItemInTheBasket()
        {
            Basket basket = new Basket(Guid.NewGuid());

            basket.AddItem(CreateFakeProduct(), 5);


            basket.LineItems.Count().Should().Be(1);
            basket.LineItems.First().Quantity.Should().Be(5);
        }

        [Test]
        public void ShouldUpdateItemQuantityIfProductIsExist()
        {
            Product fakeProdcut = CreateFakeProduct();
            Basket basket = new Basket(Guid.NewGuid());
            basket.AddItem(fakeProdcut, 5);


            basket.AddItem(fakeProdcut, 5);

            basket.LineItems.Count().Should().Be(1);
            basket.LineItems.First().Quantity.Should().Be(10);
        }


        [Test]
        public void ShouldRemoveItemFromTheBasket()
        {
            //arrange
            var fakeProduct = CreateFakeProduct();
            Basket basket = new Basket(Guid.NewGuid());
            basket.AddItem(fakeProduct, 1);

            basket.RemoveItem(basket.LineItems.First().Id);

            basket.LineItems.Count().Should().Be(0);

        }

        [Test]
        public void ShouldThrowExceptionWhileTryingToRemoveNonExistingItem()
        {
            var fakeProduct = CreateFakeProduct();
            var basket = new Basket(Guid.NewGuid());


            Action act = () => basket.RemoveItem(fakeProduct);

            act.Should().ThrowExactly<InvalidOperationException>();
        }


        private Product CreateFakeProduct()
        {
            return new Product(Guid.NewGuid(), "FakeName", "FakeSku", 50);
        }

        private List<Product> GenerateFakeProducts()
        {
            return new List<Product>
            {
                new Product(Guid.NewGuid(), "FakeName", "FakeSku", 20),
                new Product(Guid.NewGuid(), "FakeName", "FakeSku", 30),
                new Product(Guid.NewGuid(), "FakeName", "FakeSku", 40),
                new Product(Guid.NewGuid(), "FakeName", "FakeSku", 50)
            };
        }

    }
}