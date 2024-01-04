using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
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
            var attributes = await CreateFakeSpecificationAttributes();

            var fakeProduct = new Product
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Price = 50,

                ProductImages = new List<ProductImage>
                {
                    new ProductImage
                    {
                        Image = Guid.NewGuid().ToString()
                    }
                },

                SpecificationAttributes = attributes.Select(x=> new ProductSpecificationAttribute
                {
                    AttributeId = x.Id,
                    OptionId  = x.Options.First().Id
                }).ToList()
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

        public async Task<List<Manufacturer>> CreateFakeManufacturers()
        {
            var data = new List<Manufacturer>
            {
                new Manufacturer{Name = Guid.NewGuid().ToString()},
                new Manufacturer{Name = Guid.NewGuid().ToString()},
                new Manufacturer{Name = Guid.NewGuid().ToString()}
            };

            await InsertMany(data);


            return data;
        }

        public async Task<List<Tag>> CreateFakeProductTags()
        {
            var insertedData = new List<Tag>
            {
                new Tag { Name = Guid.NewGuid().ToString() },
                new Tag { Name = Guid.NewGuid().ToString() },
                new Tag { Name = Guid.NewGuid().ToString() },
            };

            var unInsertedData = new List<Tag>
            {
                new Tag { Name = Guid.NewGuid().ToString() },
                new Tag { Name = Guid.NewGuid().ToString() },
                new Tag { Name = Guid.NewGuid().ToString() },
            };

            await InsertMany(insertedData);

            var data = new List<Tag>();

            data.AddRange(insertedData);

            data.AddRange(unInsertedData);

            return data;
        }

        public async Task<List<SpecificationAttribute>> CreateFakeSpecificationAttributes()
        {
            var data = new List<SpecificationAttribute>{
                new SpecificationAttribute
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Options = new List<SpecificationAttributeOption>
                    {
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                    },
                } ,
                new SpecificationAttribute
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Options = new List<SpecificationAttributeOption>
                    {
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                    },
                },
                new SpecificationAttribute
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Options = new List<SpecificationAttributeOption>
                    {
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                        new SpecificationAttributeOption { Name = Guid.NewGuid().ToString()},
                    },
                }

            };

            await InsertMany(data);

            return data;
        }

        public async Task<SpecificationAttribute> CreateFakeSpecificationAttribute()
        {
            var attribute = new SpecificationAttribute
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Options = new List<SpecificationAttributeOption>
                {
                    new SpecificationAttributeOption
                    {
                        Name = Guid.NewGuid().ToString(),
                    }
                }
            };

            await Insert(attribute);

            return attribute;
        }
        public async Task<ProductModel> GenerateProductModel()
        { 
            var categories = await CreateFakeCategories();

            var manufacturers = await CreateFakeManufacturers();

            var productTags = await CreateFakeProductTags();

            var specificationAttributes = await CreateFakeSpecificationAttributes();

            return new ProductModel
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                IsFeatured = true,
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 150,
                Weight = new WeightModel
                {
                    Value = 50,
                    Unit = WeightUnit.Gram.ToString()
                },

                Dimensions = new DimensionModel
                {
                    Height = 5,
                    Width = 5,
                    Length = 5,
                    Unit = DimensionUnit.Inch.ToString()
                },

                Categories =categories.Select(x=> x.Id).ToHashSet(),

                Manufacturers  = manufacturers.Select(x=>x.Id).ToHashSet(),

                ProductTags = manufacturers.Select(x=> x.Name).ToHashSet(),

                SpecificationAttributes = specificationAttributes
                .Select(x=> new ProductSpecificationAttributeModel { AttributeId = x.Id, OptionId = x.Options.First().Id}).ToHashSet()

            };
        }

       
    }


}
