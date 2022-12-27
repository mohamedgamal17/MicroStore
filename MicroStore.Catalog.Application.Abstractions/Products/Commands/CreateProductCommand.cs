using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class CreateProductCommand : ProductCommandBase, ICommandV1
    {
        public ImageModel ImageModel { get; set; }

    }
    internal class CreateProductCommandValidation : ProductCommandValidatorBase<CreateProductCommand>
    {

        private readonly IRepository<Product> _productRepository;



        public CreateProductCommandValidation(IRepository<Product> productRepository, IRepository<Category> categoryRepository) : base(categoryRepository)
        {

            _productRepository = productRepository;

            RuleFor(x => x.Name)
                .MustAsync(CheckProductName)
                .WithMessage("Product name must be unique");


            RuleFor(x => x.Sku)
                .MustAsync(CheckProductSku)
                .WithMessage("Product sku must be unique");


        }




        private async Task<bool> CheckProductName(string name, CancellationToken cancellationToken)
        {
            return await _productRepository
                .AllAsync(x => x.Name != name);
        }

        private async Task<bool> CheckProductSku(string sku, CancellationToken cancellationToken)
        {
            return await _productRepository
                .AllAsync(x => x.Sku != sku);
        }


    }
}
