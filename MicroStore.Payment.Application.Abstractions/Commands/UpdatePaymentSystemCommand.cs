using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class UpdatePaymentSystemCommand : ICommand<PaymentSystemDto>
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }


    internal class UpdatePaymentSystemCommandValidator: AbstractValidator<UpdatePaymentSystemCommand>
    {
        public UpdatePaymentSystemCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Payment system name is required")
                .MaximumLength(265)
                .WithMessage("Payment system name maxmimum characters is 265");
        }
    }
}
