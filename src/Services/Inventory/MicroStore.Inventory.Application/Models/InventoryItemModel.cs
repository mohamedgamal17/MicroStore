#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.Inventory.Application.Models
{
    public class InventoryItemModel
    {
        public int Stock { get; init; }

    }

    public class InventoryItemModelValidation : AbstractValidator<InventoryItemModel>
    {
        public InventoryItemModelValidation()
        {
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock can not be negative number");

        }
    }
}
