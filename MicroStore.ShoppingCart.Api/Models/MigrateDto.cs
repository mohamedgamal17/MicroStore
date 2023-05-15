﻿using FluentValidation;

namespace MicroStore.ShoppingCart.Api.Models
{
    public class MigrateDto
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }

    }

    public class MigrateDtoValidator : AbstractValidator<MigrateDto>
    {
        public MigrateDtoValidator()
        {
            RuleFor(x => x.FromUserId)
                .NotEmpty()
                .WithMessage("From User Id Cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("From User Id maximum lenght is  256");

            RuleFor(x => x.ToUserId)
                .NotEmpty()
                .WithMessage("To User Id cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("To User Id maximum lenght is 256");
        }
    }


}
