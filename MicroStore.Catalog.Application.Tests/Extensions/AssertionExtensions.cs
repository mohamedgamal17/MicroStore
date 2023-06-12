using FluentAssertions;
using MicroStore.Catalog.Application.Models.Categories;
using MicroStore.Catalog.Application.Models.Manufacturers;
using MicroStore.Catalog.Application.Models.ProductReviews;
using MicroStore.Catalog.Application.Models.Products;
using MicroStore.Catalog.Application.Models.ProductTags;
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
            product.IsFeatured.Should().Be(model.IsFeatured);

            product.ProductCategories.OrderBy(x => x.CategoryId).Should().Equal(model.CategoriesIds?.OrderBy(x => x), (left, right) =>
            {
                return left.CategoryId == right;
            });

            product.ProductManufacturers.OrderBy(x => x.ManufacturerId).Should().Equal(model.ManufacturersIds?.OrderBy(x => x), (left, right) =>
            {
                return left.ManufacturerId == right;
            });

            product.ProductTags.OrderBy(x => x.Name).Should().Equal(model.ProductTags?.OrderBy(x => x), (left, right) =>
            {
                return left.Name == right;
            });

        }

        public static void AssertCategoryModel(this Category category, CategoryModel model)
        {

            category.Name.Should().Be(model.Name);
            category.Description.Should().Be(model.Description);
        }

        public static void AssertManufacturerModel(this Manufacturer manufacturer , ManufacturerModel model)
        {
            manufacturer.Name.Should().Be(model.Name);
            manufacturer.Description.Should().Be(model.Description);
        }

        public static void AssertProductReviewModel(this ProductReview productReview,  ProductReviewModel model)
        {
            productReview.ReviewText.Should().Be(model.ReviewText);
            productReview.Rating.Should().Be(model.Rating);
            productReview.Title.Should().Be(model.Title);
        }

        public static void AssertCreateProductReviewModel(this ProductReview productReview , CreateProductReviewModel model)
        {
            productReview.AssertProductReviewModel(model);
            productReview.UserId.Should().Be(model.UserId);
        }

        public static void AssertProductTagModel(this ProductTag productTag, ProductTagModel model)
        {
            productTag.Name.Should().Be(model.Name);
            productTag.Description.Should().Be(model.Description);
        }
    }
}
