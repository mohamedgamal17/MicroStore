namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class UpdateProductImageCommand : ProductImageCommandBase
    {
        public Guid ProductImageId { get; set; }

        internal class UpdateProductImageCommandValidator : ProductImageCommandValidatorBase<UpdateProductImageCommand> { }
    }

  
}
