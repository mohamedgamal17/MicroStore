using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class CreateProductCommand : ProductCommandBase, ICommand<ProductDto>
    {
        

    }
    internal class CreateProductCommandValidation : ProductCommandValidatorBase<CreateProductCommand>
    {


        public CreateProductCommandValidation(IRepository<Product> productRepository, IImageService imageService) 
            :base(imageService)
        {


            RuleFor(x => x.Name)
                .MustAsync((x,ct) => CheckProductName(productRepository,x,ct))
                .WithMessage("Product name must be unique");


            RuleFor(x => x.Sku)
                .MustAsync((x,ct)=>CheckProductSku(productRepository,x,ct))
                .WithMessage("Product sku must be unique");


        }




        private  Task<bool> CheckProductName(IRepository<Product> productRepository, string name, CancellationToken cancellationToken)
        {
            return productRepository
                .AllAsync(x => x.Name != name);
        }

        private  Task<bool> CheckProductSku(IRepository<Product> productRepository, string sku, CancellationToken cancellationToken)
        {
            return productRepository
                .AllAsync(x => x.Sku != sku);
        }


    }
}
