using MicroStore.ShoppingGateway.ClinetSdk.Common;

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
        public bool IsFeatured { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public string[] CategoriesIds { get; set; }
        public string[] ManufacturersIds { get; set; }
    } 


    public class ProductListRequestOptions : PagingAndSortingRequestOptions
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Tag { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public bool IsFeatured { get; set; }
    }

}
