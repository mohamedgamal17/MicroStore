﻿using MicroStore.Shipping.Application.Abstraction.Models;

namespace MicroStore.Shipping.WebApi.Models.Rates
{
    public class EstimateShipmentRateModel
    {
        public AddressModel Address { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }
    }
}