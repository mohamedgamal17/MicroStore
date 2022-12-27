﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class EstimateShipmentRateCommand : ICommandV1
    {
        public AddressModel Address { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }

    }
}
