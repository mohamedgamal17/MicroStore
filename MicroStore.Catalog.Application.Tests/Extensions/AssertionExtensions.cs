using FluentAssertions;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class AssertionExtensions
    {
        public static void AssertProductModel(this Product product , ProductModel model)
        {
            product.Sku.Should().Be(model.Sku);
            product.Name.Should().Be(model.Name);
            product.ShortDescription.Should().Be(model.ShortDescription);
            product.LongDescription.Should().Be(model.LongDescription);
            product.Price.Should().Be(model.Price);
            product.OldPrice.Should().Be(model.OldPrice);
            product.Weight.Should().Be(model.Weight.AsWeight());
            product.Dimensions.Should().Be(model.Dimensions.AsDimension());
            product.Thumbnail.Should().Be(model.Thumbnail);
            product.ProductCategories.OrderBy(x => x.CategoryId).Should().Equal(model.Categories?.OrderBy(x => x.CategoryId), (left, right) =>
            {
                return left.CategoryId == right.CategoryId &&
                    left.IsFeaturedProduct == right.IsFeatured;
            });

        }

        public static void AssertCategoryModel(this Category category, CategoryModel model)
        {

            category.Name.Should().Be(model.Name);
            category.Description.Should().Be(model.Description);
        }

    }
}
