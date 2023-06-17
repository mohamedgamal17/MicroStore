using FluentValidation;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentModel
    {
        public string OrderId { get; set; }
        public string  OrderNumber { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }
    }


    public class ShipmentModelValidation : AbstractValidator<ShipmentModel>
    {
        public ShipmentModelValidation()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Order Id cannot be null or empty")
                .Must((orderId) => Guid.TryParse(orderId, out _))
                .WithMessage("Order Id must be valid guid");


            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Order Number cannot be null or empty");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .MaximumLength(256)
                .WithMessage("User id maximum lenght is 256");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address cannot be null")
                .SetValidator(new AddressModelValidator());

            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Items cannot be null")
                .Must(items => items.Count > 0)
                .WithMessage("Shipment mus contain at least one item");


            RuleForEach(x => x.Items)
                .SetValidator(new ShipmentItemModelValidator());
                


        }
    }
}
