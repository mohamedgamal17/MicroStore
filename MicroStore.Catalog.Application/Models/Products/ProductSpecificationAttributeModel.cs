using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Models.Products
{
    public class ProductSpecificationAttributeModel
    {
        public string AttributeId { get; set; }

        public string OptionId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var right = obj as ProductSpecificationAttributeModel;

            if(right == null) return false;

            return AttributeId == right.AttributeId && OptionId == right.OptionId;
        }

        public override int GetHashCode()
        {
            return AttributeId.GetHashCode() + OptionId.GetHashCode();
        }
    }

    public class ProductSpecificationAttributeModelValidator: AbstractValidator<ProductSpecificationAttributeModel>
    {
        private readonly IRepository<SpecificationAttribute> _specificationAttributeRepository;
        public ProductSpecificationAttributeModelValidator(IRepository<SpecificationAttribute> specificationAttributeRepository)
        {
            _specificationAttributeRepository = specificationAttributeRepository;


            RuleFor(x => x.AttributeId)
                .NotEmpty()
                .WithMessage("AttributeId cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("AttributeId maximum length is 256")
                .MustAsync(async (attributeId, ct) =>
                {
                    return await _specificationAttributeRepository.AnyAsync(x => x.Id == attributeId);
                })
                .WithMessage("AttributedId is not exist");


            RuleFor(x => x.OptionId)
                .NotEmpty()
                .WithMessage("OptionId cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("OptionId maximum length is 256")
                .MustAsync(async (model, optionId, ct) =>
                {
                    var query = await _specificationAttributeRepository.GetQueryableAsync();

                    return await query.Where(x => x.Id == model.AttributeId).SelectMany(x => x.Options).AnyAsync(x => x.Id == optionId);
                })
                .WithMessage("OptionId is not exist current attribute id");

        }
    }
}
