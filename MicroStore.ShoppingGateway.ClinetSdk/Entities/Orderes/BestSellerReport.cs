﻿using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class BestSellerReport 
    {
        public string ProductId { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }

    public class BestSellerReportAggregate
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double Amount { get; set; }
        public double Quantity { get; set; }
    }
}
