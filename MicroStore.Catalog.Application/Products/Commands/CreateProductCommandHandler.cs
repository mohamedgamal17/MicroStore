﻿using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Products.Commands
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;
        public CreateProductCommandHandler(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

        }

        public override async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = new Product(request.Sku, request.Name, request.Price);

            product.SetProductShortDescription(request.ShortDescription);

            product.SetProductLongDescription(request.LongDescription);

            product.SetProductOldPrice(request.OldPrice);

            request.ProductCategories.ForEach((productCategory) =>
            {
                product.AddOrUpdateProductCategory(productCategory.CategoryId, productCategory.IsFeatured);
            });

            await _productRepository.InsertAsync(product, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }


    }
}
