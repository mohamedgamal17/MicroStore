using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Grpc.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.Categories;

namespace MicroStore.Bff.Shopping.Services.Catalog
{
    public class CategoryService  
    {
        private readonly Grpc.Catalog.CategoryService.CategoryServiceClient _categoryServiceClient;

        public CategoryService(Grpc.Catalog.CategoryService.CategoryServiceClient categoryServiceClient)
        {
            _categoryServiceClient = categoryServiceClient;
        }

        public async Task<List<Category>> ListAsync(string name = "", string sortBy = "", bool desc= false , CancellationToken cancellationToken = default)
        {
            var request = new CategoryListRequest
            {
                Name = name,
                SortBy = sortBy,
                Desc = desc
            };

            var response = await _categoryServiceClient.GetListAsync(request);

            var result = response.Data.Select(PrepareCategory).ToList();

            return result;
        }

        public async Task<Category> GetAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var request = new GetCategoryByIdRequest { Id = categoryId };

            var response = await _categoryServiceClient.GetByIdAsync(request);

            return PrepareCategory(response);
        }

        public async Task<Category> CreateAsync(CategoryModel model , CancellationToken cancellationToken = default)
        {
            var request = new CreateCategoryRequest
            {
                Name = model.Name,
                Description = model.Description
            };

            var response = await _categoryServiceClient.CreateAsync(request);

            return PrepareCategory(response);
        }

        public async Task<Category> UpdateAsync(string categoryId, CategoryModel model ,CancellationToken cancellationToken = default)
        {
            var request = new UpdateCategoryRequest
            {
                Id = categoryId,
                Name = model.Name,
                Description = model.Description
            };

            var response = await _categoryServiceClient.UpdateAsync(request);

            return PrepareCategory(response);
        }


        public Category PrepareCategory(CategoryResponse response)
        {
            return new Category
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };
        }
    }
}
