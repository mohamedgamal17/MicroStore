using FluentAssertions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.Events;

namespace MicroStore.Catalog.Domain.Tests
{
    [TestFixture]
    public class ProductTests
    {

        [Test]
        public void ShouldCreateNewProductAndEmitNewLocalEvent()
        {
            Product product = new Product("FakeSku", "FakeName", 50);

            product.GetLocalEvents().Last().EventData.Should().BeOfType<CreateProductEvent>();

        }

        [Test]
        public void ShouldAdjustProductNameAndEmitNewLocalEvent()
        {
            Product product = new Product("Fake", "Fake", 50);

            product.AdjustProductName("newname");

            product.Name.Should().Be("newname");

            product.GetLocalEvents().Last().EventData.Should().BeOfType<AdjustProductNameEvent>();
        }

        [Test]
        public void ShouldAdjustProductSkuAndEmitNewLocalEvent()
        {
            Product product = new Product("Fake", "Fake", 50);

            product.AdjustProductSku("newsku");

            product.Sku.Should().Be("newsku");

            product.GetLocalEvents().Last().EventData.Should().BeOfType<AdjustProductSkuEvent>();
        }

        [Test]
        public void ShouldAdjustProductPriceAndEmitNewLocalEvent()
        {
            Product product = new Product("Fake", "Fake", 50);

            product.AdjustProductPrice(150);

            product.Price.Should().Be(150);

            product.GetLocalEvents().Last().EventData.Should().BeOfType<AdjustProductPriceEvent>();
        }

        [Test]
        public void ShouldAddNewProductCategory()
        {
            Product product = new Product("fake", "fake", 50);

            product.AddOrUpdateProductCategory(new Category("fakecategory"), true);

            product.ProductCategories.Count.Should().Be(1);

            product.ProductCategories.Last().IsFeaturedProduct.Should().BeTrue();
        }

        [Test]
        public void ShouldUpdateExistProductCategory()
        {
            //arrange
            Product product = new Product("fake", "fake", 50);
            Category fakecategory = new Category("fake");
            product.AddOrUpdateProductCategory(fakecategory, false);

            //act
            product.AddOrUpdateProductCategory(fakecategory, true);

            //assert
            product.ProductCategories.Count.Should().Be(1);
            product.ProductCategories.Last().IsFeaturedProduct.Should().BeTrue();

        }


    }
}