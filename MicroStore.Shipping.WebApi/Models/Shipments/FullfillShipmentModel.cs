﻿using MicroStore.Shipping.Application.Abstraction.Models;

namespace MicroStore.Shipping.WebApi.Models.Shipments
{
    public class FullfillShipmentModel
    {
        public string SystemName { get; set; }
        public AddressModel AddressFrom { get; set; }
        public PackageModel Pacakge { get; set; }
    }
}