using FluentValidation;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class EstimatedRateModel
    {
        public AddressModel Address { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }
    }

    public class EstimateRateModelValidation : AbstractValidator<EstimatedRateModel>
    {
        public EstimateRateModelValidation()
        {

            RuleFor(x => x.Address)
                .NotNull()
                .SetValidator(new AddressModelValidator());

            RuleFor(x => x.Items)
                .NotNull()
                .Must(x => x.Count > 0)
                .WithMessage("Items should at least contain one item");


            RuleForEach(x => x.Items)          
                .SetValidator(new ShipmentItemEstimationModelValidator());

        }
    }

}
