using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
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

        public async Task<ResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProduct(model);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ProductDto>(validationResult.Exception);
            }

            Product product = new Product();

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ResultV2<ProductDto>> UpdateAsync(string id, ProductModel model, CancellationToken cancellationToken = default)
        {
          
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            var validationResult = await ValidateProduct( model,id);

            if (validationResult.IsFailure)
            {
                return new ResultV2<ProductDto>(validationResult.Exception);
            }

            if (product == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(Product) , id));

            }

            PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
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

            if (model.CategoriesIds != null)
            {
                product.ProductCategories = model.CategoriesIds
                    .Select(x => new ProductCategory { CategoryId = x })
                    .ToList();
            }


        }


       public async Task<ResultV2<ProductDto>> AddProductImageAsync(string productId, CreateProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);


            if (product == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(Product), productId));

            }

            ProductImage productImage = new ProductImage
            {
                ImagePath = model.Image,
                DisplayOrder = model.DisplayOrder,
            };

            product.ProductImages.Add(productImage);


            await _productRepository.UpdateAsync(product, cancellationToken : cancellationToken);


            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<ResultV2<ProductDto>> UpdateProductImageAsync(string productId, string productImageId, UpdateProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(Product), productId));

            }
            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if(productImage == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(ProductImage), productImageId));
            }

            productImage.DisplayOrder = model.DisplayOrder;

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }
        public async Task<ResultV2<ProductDto>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(Product), productId));

            }
            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if (productImage == null)
            {
                return new ResultV2<ProductDto>(new EntityNotFoundException(typeof(ProductImage), productImageId));
            }


            product.ProductImages.Remove(productImage);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        private async Task<ResultV2<Unit>> ValidateProduct(ProductModel model , string? productId = null)
        {
            var query = await _productRepository.GetQueryableAsync();

            if(productId != null)
            {
                query =  query.Where(x => x.Id != productId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return new ResultV2<Unit>(new BusinessException("Product name is already exist choose another name"));
            }

            if(await query.AnyAsync(x=> x.Sku == model.Sku))
            {
                return  new ResultV2<Unit>(new BusinessException("Product sku is already exist choose another sku"));
            }


            return Unit.Value;
        }
    }


}
