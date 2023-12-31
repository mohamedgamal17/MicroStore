using FluentValidation;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Application.Models.Products
{
    public class ProductSearchByImageQueryModel : PagingQueryParams
    {
        public string Image { get; set; }
    }

    public class ProductSearchByImageQueryModelValidator : AbstractValidator<ProductSearchByImageQueryModel>
    {
        const string URL_REGEX = "(https:\\/\\/www\\.|http:\\/\\/www\\.|https:\\/\\/|http:\\/\\/)?[a-zA-Z0-9]{2,}\\.[a-zA-Z0-9]{2,}\\.[a-zA-Z0-9]{2,}(\\.[a-zA-Z0-9]{2,})?";
        public ProductSearchByImageQueryModelValidator()
        {
            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image cannot be null or empty");

              
        }
    }
}
