﻿using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Categories;
namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryCommandService
    {
        Task<Result<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default);
    }
}
