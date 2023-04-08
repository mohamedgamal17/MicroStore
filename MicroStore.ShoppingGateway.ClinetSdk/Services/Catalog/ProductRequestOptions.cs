﻿using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductRequestOptions
    {
        public string Name { get; set; } 
        public string Sku { get; set; } 
        public string ShortDescription { get; set; } 
        public string LongDescription { get; set; } 
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public string[] CategoriesIds { get; set; }
        public string[] ManufacturersIds { get; set; }
    } 


}
