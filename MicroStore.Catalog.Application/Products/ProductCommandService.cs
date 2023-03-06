using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
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

        public async Task<UnitResult<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProduct(model);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ProductDto>(validationResult.Error);
            }

            Product product = new Product();

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<UnitResult<ProductDto>> UpdateAsync(string id, ProductModel model, CancellationToken cancellationToken = default)
        {
          
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            var validationResult = await ValidateProduct( model,id);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<ProductDto>(validationResult.Error);
            }
            if (product == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {id} is not found"));

            }

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Product, ProductDto>(product));
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
            product.IsFeatured = model.IsFeatured;

            if (model.Categories != null)
            {
                product.ProductCategories = model.Categories
                    .Select(x => new ProductCategory { CategoryId = x.CategoryId })
                    .ToList();
            }


        }


       public async Task<UnitResult<ProductDto>> AddProductImageAsync(string productId, CreateProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if(product == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {productId} is not found"));
            }

            ProductImage productImage = new ProductImage
            {
                ImagePath = model.Image,
                DisplayOrder = model.DisplayOrder,
            };

            product.ProductImages.Add(productImage);


            await _productRepository.UpdateAsync(product, cancellationToken : cancellationToken);


            return UnitResult.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<UnitResult<ProductDto>> UpdateProductImageAsync(string productId, string productImageId, UpdateProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {productId} is not found"));
            }

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if(productImage == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product image entity with id : {productImage} is not exist in product with id : {productId}"));
            }

            productImage.DisplayOrder = model.DisplayOrder;

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }
        public async Task<UnitResult<ProductDto>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product entity with id : {productId} is not found"));
            }

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if (productImage == null)
            {
                return UnitResult.Failure<ProductDto>(ErrorInfo.NotFound($"Product image entity with id : {productImage} is not exist in product with id : {productId}"));
            }

            product.ProductImages.Remove(productImage);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

        private async Task<UnitResult> ValidateProduct(ProductModel model , string? productId = null)
        {
            var query = await _productRepository.GetQueryableAsync();

            if(productId != null)
            {
                query =  query.Where(x => x.Id != productId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic("Product name is already exist choose another name"));
            }

            if(await query.AnyAsync(x=> x.Sku == model.Sku))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic("Product sku is already exist choose another sku"));
            }


            return UnitResult.Success();
        }
    }


}
