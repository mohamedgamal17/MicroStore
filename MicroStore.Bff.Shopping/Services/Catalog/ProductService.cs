using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Grpc.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.Products;
using NUglify.Helpers;

namespace MicroStore.Bff.Shopping.Services.Catalog
{
    public class ProductService
    {

        private readonly Grpc.Catalog.ProductService.ProductServiceClient _productServiceClient;

        public ProductService(Grpc.Catalog.ProductService.ProductServiceClient productServiceClient)
        {
            _productServiceClient = productServiceClient;
        }

        public async Task<Product> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var request = PrepareCreateProductRequest(model);

            var response = await _productServiceClient.CreateAsync(request);

            return PrepareProduct(response);
        }

        public async Task<Product> UpdateAsync(string productId, ProductModel model ,CancellationToken cancellationToken = default)
        {
            var request = PrepareUpdateProductRequest(productId, model);

            var response = await _productServiceClient.UpdateAsync(request);

            return PrepareProduct(response);
        }

        public async Task<PagedList<Product>> ListAsync(string name = "", string categories ="" , string manufacturers ="",  string tags ="",  bool isFeatured  =false, double minPrice = -1 , double maxPrice = -1,
            int skip  = 0, int length = 10 , string sortBy = "", bool desc = false, CancellationToken cancellationToken =default)
        {
            var request = new ProductListRequest
            {
                Name = name,
                Category = categories,
                Manufacturer = manufacturers,
                Tag = tags,
                IsFeatured = isFeatured,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Skip = skip,
                Length = length,
                SortBy = sortBy,
                Desc = desc
            };

            var response = await _productServiceClient.GetListAsync(request);

            var paged = new PagedList<Product>
            {
                Length = response.Length,
                Skip = response.Skip,
                TotalCount = response.TotalCount,
                Items = response.Items.Select(PrepareProduct)
            };

            return paged;
        }

        public async Task<Product> GetAsync(string productId , CancellationToken cancellationToken = default)
        {
            var request = new GetProductByIdRequest { Id = productId };

            var response = await _productServiceClient.GetByIdAsync(request);

            return PrepareProduct(response);
        }

        public async Task<ProductImage> CreateProductImageAsync(string productId,  ProductImageModel model , CancellationToken cancellationToken = default)
        {
            var request = new CreateProductImageRequest
            {
                ProductId = productId,
                Image = model.Image,
                DisplayOrder = model.DisplayOrder
            };

            var response = await _productServiceClient.CreateProductImageAsync(request);

            return new ProductImage
            {
                Id = response.Id,
                Image = response.Image,
                DisplayOrder  =response.DisplayOrder
            };
        }

        public async Task<ProductImage> UpdateProductImageAsync(string productId, string imageId,ProductImageModel model, CancellationToken cancellationToken = default)
        {
            var request = new UpdateProductImageRequest
            {
                ProductId = productId,
                ImageId = imageId,
                Image = model.Image,
                DisplayOrder = model.DisplayOrder
            };

            var response = await _productServiceClient.UpdateProductImageAsync(request);

            return new ProductImage
            {
                Id = response.Id,
                Image = response.Image,
                DisplayOrder = response.DisplayOrder
            };
        }

        public async Task DeleteProductImageAsync(string productId,  string imageId, CancellationToken cancellationToken = default)
        {
            var request = new DeleteProductImageRequest
            {
                ProductId = productId,
                ImageId = imageId
            };

            await _productServiceClient.DeleteProductImageAsync(request);

        }

        public async Task<List<ProductImage>> ListProductImageAsync(string productId , CancellationToken cancellationToken = default)
        {
            var request = new ProductImageListRequest { ProductId = productId };

            var response = await _productServiceClient.GetProductImageListAsync(request);

            return response.Items.Select(x => new ProductImage
            {
                Id = x.Id,
                Image = x.Image,
                DisplayOrder = x.DisplayOrder
            }).ToList();
        }

        public async Task<ProductImage> GetProductImageAsync(string productId,  string imageId , CancellationToken cancellationToken = default)
        {
            var request = new GetProductImageByIdRequest { ProductId = productId, ProductImageId = imageId };

            var response = await _productServiceClient.GetProductImageByIdAsync(request);

            return new ProductImage
            {
                Id = response.Id,
                Image = response.Image,
                DisplayOrder = response.DisplayOrder
            };
        }

        private Product PrepareProduct(ProductResponse response)
        {
            var product = new Product
            {
                Id = response.Id,
                Name = response.Name,
                Sku = response.Sku,
                ShortDescription = response.ShortDescription,
                LongDescription = response.LongDescription,
                IsFeatured = response.IsFeatured,
                OldPrice = response.OldPrice,
                Price = response.Price
            };

            if (response.Weight != null)
            {
                product.Weight = new Bff.Shopping.Data.Common.Weight
                {
                    Unit = (Data.Common.WeightUnit)response.Weight.Unit,
                    Value = response.Weight.Value
                };
            }

            if (response.Dimensions != null)
            {
                product.Dimensions = new Data.Common.Dimension
                {
                    Unit = (Data.Common.DimensionUnit)response.Dimensions.Unit,
                    Length = response.Dimensions.Length,
                    Height = response.Dimensions.Height,
                    Width = response.Dimensions.Width
                };
            }

            if (response.Categories != null)
            {
                product.Categories = response.Categories.Select(x => new ProductCategory
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
            }

            if(response.Manufacturers != null)
            {
                product.Manufacturers = response.Manufacturers.Select(x => 
                new ProductManufacturer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
            }

            if (response.Tags != null)
            {
                product.Tags = response.Tags.Select(x =>
                new ProductTag
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }

            if(response.Images != null)
            {
                product.Images = response.Images.Select(x =>
                new ProductImage
                {
                    Id = x.Id,
                    Image = x.Image,
                    DisplayOrder = x.DisplayOrder
                }).ToList();
            }

            return product;

        }
        private CreateProductRequest PrepareCreateProductRequest(ProductModel model)
        {
            var request = new CreateProductRequest
            {
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                IsFeatured = model.IsFeatured,
                OldPrice = model.OldPrice,
                Price = model.Price
            };

            if (model.Weight != null)
            {
                request.Weight = new Weight
                {
                    Unit = (WeightUnit)model.Weight.Unit,
                    Value = model.Weight.Value
                };
            }

            if (model.Dimensions != null)
            {
                request.Dimension = new Dimension
                {
                    Unit = (DimensionUnit)model.Dimensions.Unit,
                    Length = model.Dimensions.Length,
                    Height = model.Dimensions.Height,
                    Width = model.Dimensions.Width
                };
            }

            if (model.Categories != null)
            {
                request.Categories = new ProductCategoryListModel();
                model.Categories.ForEach((cat) => request.Categories.Categories.Add(cat));
            }

            if (model.Manufacturers != null)
            {
                request.Manufacturers = new ProductManufacturerListModel();
                model.Manufacturers.ForEach((man) => request.Manufacturers.Manufacturers.Add(man));
            }

            if (model.ProductTags != null)
            {
                request.Tags = new ProductTagListModel();
                model.ProductTags.ForEach((tag) => request.Tags.Tags.Add(tag));
            }

            if (model.ProductImages != null)
            {
                request.Images = new ProductImageListModel();


                var images = model.ProductImages.Select(x => new ProductImageRequest { Image = x.Image, DisplayOrder = x.DisplayOrder });

                request.Images.Images.AddRange(images);
            }

            return request;
        }

        private UpdateProductRequest PrepareUpdateProductRequest(string productId,ProductModel model)
        {
            var request = new UpdateProductRequest
            {
                Id = productId,
                Name = model.Name,
                Sku = model.Sku,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                IsFeatured = model.IsFeatured,
                OldPrice = model.OldPrice,
                Price = model.Price
            };

            if (model.Weight != null)
            {
                request.Weight = new Weight
                {
                    Unit = (WeightUnit)model.Weight.Unit,
                    Value = model.Weight.Value
                };
            }

            if (model.Dimensions != null)
            {
                request.Dimension = new Dimension
                {
                    Unit = (DimensionUnit)model.Dimensions.Unit,
                    Length = model.Dimensions.Length,
                    Height = model.Dimensions.Height,
                    Width = model.Dimensions.Width
                };
            }

            if (model.Categories != null)
            {
                request.Categories = new ProductCategoryListModel();
                model.Categories.ForEach((cat) => request.Categories.Categories.Add(cat));
            }

            if (model.Manufacturers != null)
            {
                request.Manufacturers = new ProductManufacturerListModel();
                model.Manufacturers.ForEach((man) => request.Manufacturers.Manufacturers.Add(man));
            }

            if (model.ProductTags != null)
            {
                request.Tags = new ProductTagListModel();
                model.ProductTags.ForEach((tag) => request.Tags.Tags.Add(tag));
            }

            if (model.ProductImages != null)
            {
                request.Images = new ProductImageListModel();


                var images = model.ProductImages.Select(x => new ProductImageRequest { Image = x.Image, DisplayOrder = x.DisplayOrder });

                request.Images.Images.AddRange(images);
            }

            return request;
        }
    }
}
