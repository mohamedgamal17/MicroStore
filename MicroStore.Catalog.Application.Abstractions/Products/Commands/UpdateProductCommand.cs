using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class UpdateProductCommand : ProductCommandBase, ICommand
    {
        public Guid ProductId { get; set; }

        public ImageModel ImageModel { get; set; }

    }



    internal class UpdateProductCommandCommandValidation : ProductCommandValidatorBase<UpdateProductCommand>
    {

        private readonly IRepository<Product> _productRepository;



        public UpdateProductCommandCommandValidation(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product Id Is Required");

            RuleFor(x => x.Name)
                .MustAsync(CheckProductName)
                .WithMessage("Product name must be unique");


            RuleFor(x => x.Sku)
                .MustAsync(CheckProductSku)
                .WithMessage("Product sku must be unique");


        }




        private async Task<bool> CheckProductName(UpdateProductCommand command, string name, CancellationToken cancellationToken)
        {
            var query = await _productRepository.GetQueryableAsync();

            return await  query.Where(x => x.Id != command.ProductId)
                .AllAsync(x => x.Name != name , cancellationToken);
        }

        private async Task<bool> CheckProductSku(UpdateProductCommand command, string sku, CancellationToken cancellationToken)
        {
            var query = await _productRepository.GetQueryableAsync();

            return await query.Where(x => x.Id != command.ProductId)
                .AllAsync(x => x.Sku != sku, cancellationToken);
        }


    }
}
