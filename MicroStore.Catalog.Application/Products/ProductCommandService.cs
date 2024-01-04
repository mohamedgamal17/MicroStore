using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Products;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using System.Threading;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Products
{
    public class ProductCommandService : CatalogApplicationService, IProductCommandService
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<ProductTag> _productTagRepository;

        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;

        private readonly IRepository<Category> _categoryRepository;

        private readonly IRepository<Manufacturer> _manufacturerRepository;
        public ProductCommandService(IRepository<Product> productRepository, IRepository<ProductTag> productTagRepository, IRepository<SpecificationAttribute> specificationAttributeRepository, IRepository<Category> categoryRepository, IRepository<Manufacturer> manufacturerRepository)
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _specificationAttributeRepository = specificationAttributeRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<Result<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateProduct(model);

            if (validationResult.IsFailure)
            {
                return new Result<ProductDto>(validationResult.Exception);
            }

            Product product = new Product();

            await PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<Result<ProductDto>> UpdateAsync(string id, ProductModel model, CancellationToken cancellationToken = default)
        {
          
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            var validationResult = await ValidateProduct( model,id);

            if (validationResult.IsFailure)
            {
                return new Result<ProductDto>(validationResult.Exception);
            }

            if (product == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(Product) , id));

            }

            await PrepareProductEntity(product, model, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }


        private async Task PrepareProductEntity(Product product, ProductModel model, CancellationToken cancellationToken = default)
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
                List<Category> categories = new List<Category>();

                if(model.Categories.Count > 0)
                {
                    categories = await _categoryRepository.GetListAsync(x => model.Categories.Contains(x.Id));
                }

                product.Categories = categories;
            }

            if(model.Manufacturers != null)
            {
                List<Manufacturer> manufacturers = new List<Manufacturer>();

                if(model.Manufacturers.Count > 0)
                {
                    manufacturers = await _manufacturerRepository.GetListAsync(x => model.Manufacturers.Contains(x.Id));
                }
                product.Manufacturers = manufacturers;
            }

            if(model.ProductTags != null)
            {
                product.ProductTags = await PrepareProductTags(model.ProductTags, cancellationToken);
            }

            if(model.ProductImages != null)
            {
                product.ProductImages = model.ProductImages.Select(x => new ProductImage
                {
                    Image = x.Image,
                    DisplayOrder = x.DisplayOrder,
                }).ToList();
            }

            if(model.SpecificationAttributes != null)
            {
                product.SpecificationAttributes = await PrepareProductSpecificationAttributes(model.SpecificationAttributes,cancellationToken);
            }
        }

        private async Task<List<ProductTag>> PrepareProductTags(HashSet<string> tags, CancellationToken cancellationToken )
        {
            var productTags = await _productTagRepository.GetListAsync(x => tags.Contains(x.Name), cancellationToken: cancellationToken);

            foreach (var tag in tags.Where(x => !productTags.Select(c => c.Name).Contains(x)))
            {
                productTags.Add(new ProductTag { Name = tag });
            }

            return productTags;
        }


        private async Task<List<ProductSpecificationAttribute>> PrepareProductSpecificationAttributes(HashSet<ProductSpecificationAttributeModel> specificationAttributes, CancellationToken cancellationToken)
        {
            var query = await _specificationAttributeRepository.GetQueryableAsync();

            var attributesIds = specificationAttributes.Select(x => x.AttributeId).ToList();

            specificationAttributes = specificationAttributes.OrderBy(x => x.AttributeId).ToHashSet();

            var attributes = await query.Include(x => x.Options).Where(x=> attributesIds.Contains(x.Id)).OrderBy(x=> x.Id).ToArrayAsync(cancellationToken);

            var productSpecificationAttributes = new List<ProductSpecificationAttribute>();

            foreach(var tuple in Enumerable.Zip(specificationAttributes, attributes))
            {
                productSpecificationAttributes.Add(new ProductSpecificationAttribute
                {
                    Attribute = tuple.Second,
                    Option = tuple.Second.Options.Single(x => x.Id == tuple.First.OptionId)
                });
            }

            return productSpecificationAttributes;

        }

       public async Task<Result<ProductImageDto>> AddProductImageAsync(string productId, ProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);


            if (product == null)
            {
                return new Result<ProductImageDto>(new EntityNotFoundException(typeof(Product), productId));

            }

            ProductImage productImage = new ProductImage
            {
                Image = model.Image,
                DisplayOrder = model.DisplayOrder,
                ProductId = product.Id
            };

            product.ProductImages.Add(productImage);


            await _productRepository.UpdateAsync(product, cancellationToken : cancellationToken);


            return ObjectMapper.Map<ProductImage, ProductImageDto>(productImage);
        }

        public async Task<Result<ProductImageDto>> UpdateProductImageAsync(string productId, string productImageId, ProductImageModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return new Result<ProductImageDto>(new EntityNotFoundException(typeof(Product), productId));

            }
            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if(productImage == null)
            {
                return new Result<ProductImageDto>(new EntityNotFoundException(typeof(ProductImage), productImageId));
            }

            productImage.Image = model.Image;

            productImage.DisplayOrder = model.DisplayOrder;

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<ProductImage, ProductImageDto>(productImage);
        }
        public async Task<Result<Unit>> DeleteProductImageAsync(string productId, string productImageId, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository
                .SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(Product), productId));

            }
            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == productImageId);

            if (productImage == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(ProductImage), productImageId));
            }


            product.ProductImages.Remove(productImage);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        private async Task<Result<Unit>> ValidateProduct(ProductModel model , string? productId = null)
        {
            var query = await _productRepository.GetQueryableAsync();

            if(productId != null)
            {
                query =  query.Where(x => x.Id != productId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return new Result<Unit>(new UserFriendlyException("Product name is already exist choose another name"));
            }

            if(await query.AnyAsync(x=> x.Sku == model.Sku))
            {
                return  new Result<Unit>(new UserFriendlyException("Product sku is already exist choose another sku"));
            }


            return Unit.Value;
        }

        public async Task<Result<ProductDto>> CreateProductAttributeSpecificationAsync(string productId, ProductSpecificationAttributeModel model, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if(product== null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(Product)));
            }

            var attributeQuery = await _specificationAttributeRepository.WithDetailsAsync(x=> x.Options);

            var attribute = await attributeQuery.SingleAsync(x => x.Id == model.AttributeId,cancellationToken);

            var option = attribute.Options.Single(x => x.Id == model.OptionId);

            var productSpecificationAttrribute = new ProductSpecificationAttribute
            {
                Attribute = attribute,
                Option = option
            };

            product.SpecificationAttributes.Add(productSpecificationAttrribute);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        public async Task<Result<ProductDto>> RemoveProductAttributeSpecificationAsync(string productId, string attributeId, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.SingleOrDefaultAsync(x => x.Id == productId, cancellationToken);

            if (product == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(Product),productId));
            }

            var productSpecificationAttribute = product.SpecificationAttributes.SingleOrDefault(x => x.Id == attributeId);

            if(productSpecificationAttribute == null)
            {
                return new Result<ProductDto>(new EntityNotFoundException(typeof(ProductSpecificationAttribute), attributeId));
            }


            product.SpecificationAttributes.Remove(productSpecificationAttribute);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }
    }


}
