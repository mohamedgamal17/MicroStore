using FluentAssertions;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class AssertionExtensions
    {

        public static void AssertProductCommand(this Product product,  ProductCommand command)
        {
            product.Sku.Should().Be(command.Sku);
            product.Name.Should().Be(command.Name);
            product.ShortDescription.Should().Be(command.ShortDescription);
            product.LongDescription.Should().Be(command.LongDescription);
            product.Price.Should().Be(command.Price);
            product.OldPrice.Should().Be(command.OldPrice);
            product.Weight.Should().Be(command.Weight.AsWeight());
            product.Dimensions.Should().Be(command.Dimensions.AsDimension());
            product.Thumbnail.Should().Contain(command.Thumbnail.FileName);
            product.ProductCategories.OrderBy(x=> x.CategoryId).Should().Equal(command.Categories?.OrderBy(x=> x.CategoryId), (left, right) =>
            {
                return left.CategoryId == right.CategoryId &&
                    left.IsFeaturedProduct == right.IsFeatured;
            });
        }
    

        public static void AssertCategoryCommand(this Category category , CategoryCommand command)
        {

            category.Name.Should().Be(command.Name);
            category.Description.Should().Be(command.Description);
        }

    }
}
