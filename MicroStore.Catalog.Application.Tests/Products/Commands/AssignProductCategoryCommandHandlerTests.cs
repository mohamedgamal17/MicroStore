using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using System.Drawing;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Tests.Products.Commands
{
    public class AssignProductCategoryCommandHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_assign_cateogry_to_product()
        {
            Product fakeProduct = await GenerateFakeProduct();

            Category fakeCategory = await GenerateFakeCategory();

            var command = new AssignProductCategoryCommand
            {
                CategoryId = fakeCategory.Id,
                ProductId = fakeProduct.Id,
                IsFeatured = true
            };

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            result.IsSuccess.Should().BeTrue();

            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.ProductCategories.Count().Should().Be(1);

            ProductCategory productCategory = product.ProductCategories[0];

            productCategory.CategoryId.Should().Be(fakeCategory.Id);

            productCategory.IsFeaturedProduct.Should().Be(command.IsFeatured);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_when_product_is_not_exist()
        {

            var command = new AssignProductCategoryCommand
            {
                CategoryId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                IsFeatured = true
            };


            var result = await Send(command);

            result.IsFailure.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_exception_while_category_is_not_exist()
        {
            var fakeProduct = await GenerateFakeProduct();

            var command = new AssignProductCategoryCommand
            {
                CategoryId = Guid.NewGuid(),
                ProductId = fakeProduct.Id,
                IsFeatured = true
            };


            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }




        private Task<Product> GenerateFakeProduct()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, Guid.NewGuid().ToString()));
            });
        }


        private Task<Category> GenerateFakeCategory()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Category>>();

                return repository.InsertAsync(new Category(Guid.NewGuid().ToString()));
            });
        }

    }
}
