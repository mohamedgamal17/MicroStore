using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.Categories
{
    public abstract class CategoryTestBase : BaseTestFixture
    {
        public async Task<Category> CreateFakeCategory()
        {
            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
            };

            await Insert(category);

            return category;

        }
    }
}
