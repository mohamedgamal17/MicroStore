﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class UpdateProdutCommand : ICommand
    {
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }

    }
}
