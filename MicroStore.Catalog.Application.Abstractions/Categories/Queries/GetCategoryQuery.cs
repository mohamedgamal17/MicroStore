﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Queries
{
    public class GetCategoryQuery : IQuery<CategoryDto>
    {
        public Guid Id { get; set; }
    }
}