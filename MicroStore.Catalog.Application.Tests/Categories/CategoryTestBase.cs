using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Tests.Categories
{
    public abstract class CategoryTestBase : BaseTestFixture
    {
        public async Task<Category> CreateFakeCategory()
        {

            return await WithUnitOfWork(async (sp) =>
            {
                var category = new Category
                {
                    Name = Guid.NewGuid().ToString(),
                };

                var repository = sp.GetRequiredService<IRepository<Category>>();

                return await repository.InsertAsync(category);
            });

        }
    }
}
