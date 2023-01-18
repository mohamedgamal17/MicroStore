using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class RetriveShipmentRateCommand : IQuery<ListResultDto<ShipmentRateDto>>
    {
        public string SystemName { get; set; }
        public string ExternalShipmentId { get; set; }
    }


    internal class RetriveShipmentRateCommandValidator : AbstractValidator<RetriveShipmentRateCommand>
    {
        public RetriveShipmentRateCommandValidator()
        {
            RuleFor(x => x.SystemName)
                .NotEmpty()
                .WithMessage("System name is required")
                .MaximumLength(265)
                .WithMessage("System name maximum lenght is 265");

            RuleFor(x => x.ExternalShipmentId)
                .NotEmpty()
                .WithMessage("External shipment id is required")
                .MaximumLength(265)
                .WithMessage("External shipment id maximum lenght is 265");
        }
    }
}
