﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class UpdateProductCategoryCommand : ICommandV1
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }
    }
}
