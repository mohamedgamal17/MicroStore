﻿using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Products.Commands
{
    public class AssignProductCategoryComandHandler : CommandHandler<AssignProductCategoryCommand, ProductDto>
    {
        private readonly IRepository<Product> _productRepository;

        private readonly IRepository<Category> _categoryRepository;

        public AssignProductCategoryComandHandler(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public override async Task<ProductDto> Handle(AssignProductCategoryCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);

            if(product == null)
            {
                throw new EntityNotFoundException(typeof(Product),request.ProductId);
            }

            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);

            if(category == null)
            {
                throw new EntityNotFoundException(typeof(Category),request.CategoryId);
            }

            product.AddOrUpdateProductCategory(category, request.IsFeatured);

            await _productRepository.UpdateAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }
    }
}