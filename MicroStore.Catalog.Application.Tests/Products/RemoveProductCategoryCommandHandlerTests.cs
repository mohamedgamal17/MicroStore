using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Tests.Products
{
    public class RemoveProductCategoryCommandHandlerTests : BaseTestFixture
    {


        [Test]
        public async Task Should_remove_product_category()
        {
            var fakeCategory = await GenerateFakeCategory();

            var fakeProduct = await GenerateFakeProduct(fakeCategory);

            var command = new RemoveProductCategoryCommand
            {
                ProductId = fakeProduct.Id,
                CategoryId = fakeCategory.Id
            };

            await Send(command);

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.ProductImages.Count.Should().Be(0);
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_while_product_is_not_exist()
        {
            var command = new RemoveProductCategoryCommand
            {
                ProductId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_throw_user_friendly_exception_while_category_is_not_assigned_to_product()
        {
            var fakeCategory = await GenerateFakeCategory();

            var fakeProduct = await GenerateFakeProduct(fakeCategory);

            var command = new RemoveProductCategoryCommand
            {
                ProductId = fakeProduct.Id,
                CategoryId = Guid.NewGuid(),
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();

        }

        private Task<Product> GenerateFakeProduct(Category category)
        {
            return WithUnitOfWork((sp) =>
            {
                var product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50);

                product.AddOrUpdateProductCategory(category, false);

                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(product);
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
