using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products
{
    public class ProductCommandService : CatalogApplicationService, IProductCommandService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductCommandService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UnitResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProduct(model);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ProductDto>(validationResult.Error);
            }

            Product product = new Product();

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<UnitResultV2<ProductDto>> UpdateAsync(string id, ProductModel model, CancellationToken cancellationToken = default)
        {
          
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            var validationResult = await ValidateProduct( model,id);

            if (validationResult.IsFailure)
            {
                return UnitResultV2.Failure<ProductDto>(validationResult.Error);
            }
            if (product == null)
            {
                return UnitResultV2.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {id} is not found"));

            }

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }


        private void PrepareProductEntity(Product product, ProductModel model, CancellationToken cancellationToken = default)
        {
            product.Sku = model.Sku;
            product.Name = model.Name;
            product.Price = model.Price;
            product.ShortDescription = model.ShortDescription;
            product.LongDescription = model.LongDescription;
            product.OldPrice = model.OldPrice;
            product.Weight = model.Weight?.AsWeight() ?? Weight.Empty;
            product.Dimensions = model.Dimensions?.AsDimension() ?? Dimension.Empty;
            product.Thumbnail = model.Thumbnail;

            if (model.Categories != null)
            {
                product.ProductCategories = model.Categories
                    .Select(x => new ProductCategory { CategoryId = x.CategoryId, IsFeaturedProduct = x.IsFeatured })
                    .ToList();
            }

            if (model.Images != null)
            {
                product.ProductImages = model.Images.Select(x => new ProductImage
                {
                    ImagePath = x.Image,
                    DisplayOrder = x.DisplayOrder,
                }).ToList();
            }
        }


        private async Task<UnitResultV2> ValidateProduct(ProductModel model , string? productId = null)
        {
            var query = await _productRepository.GetQueryableAsync();

            if(productId != null)
            {
                query =  query.Where(x => x.Id != productId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return UnitResultV2.Failure(ErrorInfo.BusinessLogic("Product name is already exist choose another name"));
            }

            if(await query.AnyAsync(x=> x.Sku == model.Sku))
            {
                return UnitResultV2.Failure(ErrorInfo.BusinessLogic("Product sku is already exist choose another sku"));
            }


            return UnitResultV2.Success();
        }
    }


}
