using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Products
{
    public class ProductCommandHandler : RequestHandler,
        ICommandHandler<CreateProductCommand, ProductDto>,
        ICommandHandler<UpdateProductCommand, ProductDto>,
        ICommandHandler<CreateProductImageCommand, ProductDto>,
        ICommandHandler<UpdateProductImageCommand, ProductDto>,
        ICommandHandler<RemoveProductImageCommand, ProductDto>

    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Category> _categoryRepository;

        private readonly IImageService _imageService;

        public ProductCommandHandler(IRepository<Product> productRepository, IImageService imageService, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseResult<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product();

            await PrepareProductEntity(product, request, cancellationToken);

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<ResponseResult<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository
           .SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Product entity with id : {request.ProductId} is not found" });
            }


            await  PrepareProductEntity(product, request, cancellationToken);

            await _productRepository.UpdateAsync(product, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Product, ProductDto>(product));

        }

        public async Task<ResponseResult<ProductDto>> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound, $"Product entity with id : {request.ProductId} is not found");
            }

            var imageResult = await _imageService.SaveAsync(request.ImageModel);

            if (!imageResult.IsValid)
            {
                return Failure<ProductDto>(HttpStatusCode.BadRequest, "Invalid image extension");
            }

            product.AssignProductImage(imageResult.ImageLink, request.DisplayOrder);

            await _productRepository.UpdateAsync(product);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Product, ProductDto>(product));
        }


        public async Task<ResponseResult<ProductDto>> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Product entity with id : {request.ProductId} is not found" });
            }

            var productImage = product.ProductImages.SingleOrDefault(x => x.Id == request.ProductImageId);

            if(productImage == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Product image entity with id : {request.ProductImageId} is not found" });
            }


            if (request.ImageModel != null)
            {
                ImageResult imageResult = await _imageService.SaveAsync(request.ImageModel, cancellationToken);

                if (!imageResult.IsValid)
                {
                    throw new InvalidOperationException("Invalid image extension");
                }

                productImage.ImagePath = imageResult.ImageLink;
            }

            productImage.DisplayOrder = request.DisplayOrder;

            await _productRepository.UpdateAsync(product , cancellationToken :cancellationToken);


            return Success(HttpStatusCode.OK, ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<ResponseResult<ProductDto>> Handle(RemoveProductImageCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if (product == null)
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Product entity with id : {request.ProductId} is not found" });
            }

            if (!product.ProductImages.Any(x => x.Id == request.ProductImageId))
            {
                return Failure<ProductDto>(HttpStatusCode.NotFound,
                    new ErrorInfo { Message = $"Product image entity with id : {request.ProductImageId} is not found" });
            }

            product.RemoveProductImage(request.ProductImageId);

            await _productRepository.UpdateAsync(product);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Product, ProductDto>(product));
        }

 

        private async Task PrepareProductEntity(Product product, ProductCommand request, CancellationToken cancellationToken = default)
        {
            product.Sku = request.Sku;
            product.Name = request.Name;
            product.Price = request.Price;
            product.ShortDescription = request.ShortDescription;
            product.LongDescription = request.LongDescription;
            product.OldPrice = request.OldPrice;
            product.Weight = request.Weight?.AsWeight() ?? Weight.Empty;
            product.Dimensions = request.Dimensions?.AsDimension() ?? Dimension.Empty;
            if (request.Thumbnail != null)
            {
                ImageResult imageResult = await _imageService.SaveAsync(request.Thumbnail, cancellationToken);

                if (!imageResult.IsValid)
                {
                    throw new InvalidOperationException("Invalid image extension");
                }

                product.Thumbnail = imageResult.ImageLink;
            }


            if(request.Categories != null)
            {
                product.ProductCategories = request.Categories
                    .Select(x => new ProductCategory { CategoryId = x.CategoryId, IsFeaturedProduct = x.IsFeatured })
                    .ToList();
            }
        }
    }
}
