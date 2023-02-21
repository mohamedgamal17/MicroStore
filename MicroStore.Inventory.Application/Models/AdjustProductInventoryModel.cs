#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.Inventory.Application.Models
{
    public class AdjustProductInventoryModel
    {
        public int Stock { get; init; }

        public string Reason { get; init; }
    }

    public class AdjustProductInventoryModeValidation : AbstractValidator<AdjustProductInventoryModel>
    {
        public AdjustProductInventoryModeValidation()
        {
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock can not be negative number");


            RuleFor(x => x.Reason)
                .MinimumLength(3)
                .WithMessage("Adjust product reason min lenght is 3");
        }
    }
}
