using FluentValidation;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentItemModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }
        public ShipmentItem AsShipmentItem()
        {
            return new ShipmentItem
            {
                Name = Name,
                Sku = Sku,
                ProductId = ProductId,
                Thumbnail = Thumbnail,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                Weight = Weight.AsWeight(),
                Dimension = Dimension.AsDimension()
            };
        }
    }


    internal class ShipmentItemModelValidator : AbstractValidator<ShipmentItemModel>
    {

        public ShipmentItemModelValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull()
                .WithMessage("Product id is required")
                .MaximumLength(265)
                .WithMessage("Product id maximum characters is 265");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Item name is required")
                .MaximumLength(265)
                .WithMessage("Item name maximum characters is 265");

            RuleFor(x => x.Sku)
                .NotNull()
                .WithMessage("Item sku is required")
                .MaximumLength(265)
                .WithMessage("Item sku maximum characters is 265");

            RuleFor(x => x.Thumbnail)
                .NotNull()
                .WithMessage("Item Image is required")
                .MaximumLength(500)
                .WithMessage("Item Image maximum characters is 265");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Item quantity should be greater than zero");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Item unit price should be greater than zero");


            RuleFor(x => x.Weight)
                .NotNull()
                .WithMessage("weight is required")
                .SetValidator(new WeightModelValidator());

            RuleFor(x => x.Dimension)
                .NotNull()
                .WithMessage("dimension is required")
                .SetValidator(new DimesnsionModelValidator());
        }

    }
}
