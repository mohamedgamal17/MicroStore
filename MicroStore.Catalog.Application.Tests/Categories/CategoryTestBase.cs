using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.Categories
{
    public abstract class CategoryTestBase : BaseTestFixture
    {
        public async Task<Category> CreateFakeCategory()
        {
            var fakeCategory = new Category
            {
                Name = Guid.NewGuid().ToString(),
            };

            return await Insert(fakeCategory);

        }
    }
}
