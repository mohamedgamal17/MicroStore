using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class ProductServiceTestBase : BaseTestFixture
    {

        public Task<Product> GetProductById(string id)
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.SingleAsync(x => x.Id == id);
            });
        }
        public async Task<Product> CreateFakeProduct()
        {
            var fakeProduct = new Product
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Price = 50,
                ProductCategories = new List<ProductCategory>()
                {
                   new ProductCategory
                   {
                       Category = new Category
                       {
                           Name = Guid.NewGuid().ToString(),
                       }
                   }
                },

                ProductImages = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImagePath = Guid.NewGuid().ToString()
                    }
                }
            };

            return await Insert(fakeProduct);
        }

        public async Task<Category> CreateFakeCategory()
        {
            var category = new Category
            {
                Name = Guid.NewGuid().ToString(),
            };

            return await Insert(category);
        }


        public async Task<List<Category>> CreateFakeCategories()
        {
            var data = new List<Category>
            {
                new Category {Name = Guid.NewGuid().ToString()},
                new Category {Name = Guid.NewGuid().ToString()},
                new Category {Name = Guid.NewGuid().ToString()},
                new Category {Name = Guid.NewGuid().ToString()}
            };

            await InsertMany(data);


            return data;
        }
      
        public async Task<ProductModel> GenerateProductModel()
        { 
            var categories = await CreateFakeCategories();

            return new ProductModel
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Thumbnail = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 150,
                Weight = new WeightModel
                {
                    Value = 50,
                    Unit = "g"
                },

                Dimensions = new DimensionModel
                {
                    Height = 5,
                    Width = 5,
                    Lenght = 5,
                    Unit = "inch"
                },

                Categories = categories.Select(x => new ProductCategoryModel { CategoryId = x.Id, IsFeatured = true }).ToList(),
                Images = new List<ProductImageModel> { new ProductImageModel { Image = Guid.NewGuid().ToString(), DisplayOrder = 1 } }
            };
        }

       
    }


}
