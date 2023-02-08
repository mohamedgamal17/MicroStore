using FluentValidation;
using MicroStore.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Models
{
    public class ProductCategoryModel
    {
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }

    }

    internal class ProductCategoryModelValidator : AbstractValidator<ProductCategoryModel>
    {
        private readonly IRepository<Category> _categoryRepository;
        public ProductCategoryModelValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category id cannot be empty")
                .MustAsync(CheckCategoryId)
                .WithMessage("Category is not exist");

        }


        private async Task<bool> CheckCategoryId(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _categoryRepository.AnyAsync(x => x.Id == categoryId, cancellationToken);
        }



    }
}
