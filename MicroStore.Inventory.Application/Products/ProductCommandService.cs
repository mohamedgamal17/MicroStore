﻿using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace MicroStore.Inventory.Application.Products
{
    public class ProductCommandService : InventoryApplicationService, IProductCommandService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductCommandService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [DisableValidation]
        public async Task<UnitResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default)
        {
            Product product = new Product(model.ProductId);

            PrepareProductEntity(product,model);

            await _productRepository.InsertAsync(product);

            return UnitResultV2.Success(ObjectMapper.Map<Product,ProductDto>(product));
        }

        [DisableValidation]
        public async Task<UnitResultV2<ProductDto>> UpdateAsync( ProductModel model, CancellationToken cancellationToken = default)
        {
            Product product = await _productRepository.SingleAsync(x => x.Id == model.ProductId);

            PrepareProductEntity(product,model);

            await _productRepository.UpdateAsync(product);

            return UnitResultV2.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

        public async Task<UnitResultV2<ProductDto>> AdjustInventory(string id ,AdjustProductInventoryModel model, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return UnitResultV2.Failure<ProductDto>(ErrorInfo.NotFound($"Product with id : {id} is not exist"));
            }

            product.AdjustInventory(model.Stock, model.Reason);


            await _productRepository.UpdateAsync(product);


            return UnitResultV2.Success(ObjectMapper.Map<Product, ProductDto>(product));
        }

  
        private void PrepareProductEntity(Product product , ProductModel model)
        {
            product.Name = model.Name;
            product.Sku= model.Sku;
            product.Thumbnail = model.Thumbnail;
        }
    }
}