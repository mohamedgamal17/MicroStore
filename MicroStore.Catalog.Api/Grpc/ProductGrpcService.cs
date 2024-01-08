using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Catalog.Application.Abstractions.Products;
using NUglify.Helpers;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Api.Grpc
{
    public class ProductGrpcService : ProductService.ProductServiceBase
    {
        private readonly IProductCommandService _productCommandService;

        private readonly IProductQueryService _productQueryService;

        public ProductGrpcService(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public override async Task<ProductResponse> Create(CreateProductRequest request, ServerCallContext context)
        {
            var model = PrepareProductModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException(); 
            }

            var result = await _productCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductResponse(result.Value);
        }

        public override async Task<ProductResponse> Update(UpdateProductRequest request, ServerCallContext context)
        {
            var model = PrepareProductModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productCommandService.UpdateAsync(request.Id,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductResponse(result.Value);
        }

        public override async Task<ProductListResponse> GetList(ProductListRequest request, ServerCallContext context)
        {
            var model = new ProductListQueryModel
            {
                Name = request.Name,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                Category = request.Category,
                Manufacturer = request.Manufacturer,
                Tag = request.Tag,
                IsFeatured = request.IsFeatured,
                Length = request.Length,
                Skip = request.Skip,
                SortBy = request.SortBy
            };

          

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productQueryService.ListAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductListResponse(result.Value);
        }

        public override async Task<ProductResponse> GetById(GetProductByIdRequest request, ServerCallContext context)
        {
            var result = await _productQueryService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductResponse(result.Value);
        }

        public override async Task<ProductImageResponse> CreateProductImage(CreateProductImageRequest request, ServerCallContext context)
        {
            var model = new ProductImageModel
            {
                Image = request.Image,
                DisplayOrder = request.DisplayOrder
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productCommandService.AddProductImageAsync(request.ProductId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return PrepareProductImageResponse(result.Value);
        }


        public override async Task<ProductImageResponse> UpdateProductImage(UpdateProductImageRequest request, ServerCallContext context)
        {
            var model = new ProductImageModel
            {
                Image = request.Image,
                DisplayOrder = request.DisplayOrder
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productCommandService.UpdateProductImageAsync(request.ProductId, request.ImageId,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductImageResponse(result.Value);

        }

        public override async Task<Empty> DeleteProductImage(DeleteProductImageRequest request, ServerCallContext context)
        {
            var result = await _productCommandService.DeleteProductImageAsync(request.ProductId, request.ImageId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return new Empty();
        }

        public override async Task<ProductImageListResponse> GetProductImageList(ProductImageListRequest request, ServerCallContext context)
        {
            var result = await _productQueryService.ListProductImagesAsync(request.ProductId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductImageListResponse(result.Value);
        }

        public override async Task<ProductImageResponse> GetProductImageById(GetProductImageByIdRequest request, ServerCallContext context)
        {
            var result = await _productQueryService.GetProductImageAsync(request.ProductId,request.ProductImageId);


            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareProductImageResponse(result.Value);
        }
        private ProductModel PrepareProductModel(CreateProductRequest request)
        {
            var model = new ProductModel
            {
                Name = request.Name,
                Sku = request.Sku,
                ShortDescription = request.ShortDescription,
                LongDescription = request.LongDescription,
                Price = request.Price,
                OldPrice = request.OldPrice,
                IsFeatured = request.IsFeatured,
            };

            if (request.Weight != null)
            {
                model.Weight = new WeightModel
                {
                    Value = request.Weight.Value,
                    Unit = request.Weight.Unit.ToString()
                };
            }

            if(request.Dimension != null)
            {
                model.Dimensions = new DimensionModel
                {
                    Width = request.Dimension.Width,
                    Length = request.Dimension.Length,
                    Height = request.Dimension.Height,
                    Unit = request.Dimension.Unit.ToString()
                };
            }

            model.Categories = request.Categories.ToHashSet();

            model.Manufacturers = request.Manufacturers.ToHashSet();

            model.ProductImages = request.Images.Select(x => new ProductImageModel
            {
                Image = x.Image,
                DisplayOrder = x.DisplayOrder
            }).ToList();


            return model;
        }
        private ProductModel PrepareProductModel(UpdateProductRequest request)
        {
            var model = new ProductModel
            {
                Name = request.Name,
                Sku = request.Sku,
                ShortDescription = request.ShortDescription,
                LongDescription = request.LongDescription,
                Price = request.Price,
                OldPrice = request.OldPrice,
                IsFeatured = request.IsFeatured,
            };

            if (request.Weight != null)
            {
                model.Weight = new WeightModel
                {
                    Value = request.Weight.Value,
                    Unit = request.Weight.Unit.ToString()
                };
            }

            if (request.Dimension != null)
            {
                model.Dimensions = new DimensionModel
                {
                    Width = request.Dimension.Width,
                    Length = request.Dimension.Length,
                    Height = request.Dimension.Height,
                    Unit = request.Dimension.Unit.ToString()
                };
            }

            model.Categories = request.Categories.ToHashSet();

            model.Manufacturers = request.Manufacturers.ToHashSet();

            model.ProductImages = request.Images.Select(x => new ProductImageModel
            {
                Image = x.Image,
                DisplayOrder = x.DisplayOrder
            }).ToList();


            return model;
        }

        private ProductResponse PrepareProductResponse(ProductDto product)
        {
            var response = new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                Price = product.Price,
                OldPrice = product.OldPrice,
                IsFeatured = product.IsFeatured
            };

            if(product.Weight != null)
            {
                response.Weight = new Weight
                {
                    Value = product.Weight.Value,
                    Unit = Enum.Parse<WeightUnit>(product.Weight.Unit)
                };
            }

            if(product.Dimensions != null)
            {
                response.Dimensions = new Dimension
                {
                    Length = product.Dimensions.Length,
                    Height = product.Dimensions.Height,
                    Width = product.Dimensions.Width,
                    Unit = Enum.Parse<DimensionUnit>(product.Dimensions.Unit)
                };
            }

            if (product.Categories != null)
            {
                product.Categories.ForEach(cat =>
                {
                    response.Categories.Add(new ProductCategoryResponse
                    {
                        Id = cat.Id,
                        Name = cat.Name,
                        Description = cat.Description
                    });
                });    
            }

            if(product.Manufacturers != null)
            {
                product.Manufacturers.ForEach(man =>
                {
                    response.Manufacturers.Add(new ProductManufacturerResponse
                    {
                        Id = man.Id,
                        Name = man.Name,
                        Description = man.Description
                    });
                });
            }

            if(response.Tags != null)
            {
                product.Tags.ForEach(tag =>
                {
                    response.Tags.Add(new ProductTagResponse
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                    });
                });
            }

            if(response.Images != null)
            {
                product.Images.ForEach(x =>PrepareProductImageResponse(x));
            }
            return response;
        }


        private ProductListResponse PrepareProductListResponse(PagedResult<ProductDto> pagedResult)
        {
            var response = new ProductListResponse
            {
                Skip = pagedResult.Skip,
                Length = pagedResult.Lenght,
                TotalCount = pagedResult.TotalCount
            };

            pagedResult.Items.ForEach(pr =>
            {
                response.Items.Add(PrepareProductResponse(pr));
            });


            return response;
        }

        private ProductImageResponse PrepareProductImageResponse(ProductImageDto productImageDto)
        {
            return new ProductImageResponse
            {
                Id = productImageDto.Id,
                Image = productImageDto.Image,
                DisplayOrder = productImageDto.DisplayOrder
            };
        }

        private ProductImageListResponse PrepareProductImageListResponse(List<ProductImageDto> images )
        {
            var response = new ProductImageListResponse();

            images.ForEach(x =>
            {
                response.Items.Add(PrepareProductImageResponse(x));
            });


            return response;
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }

        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
