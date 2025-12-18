namespace MicroStore.Catalog.Domain.Configuration
{
    public class ElasticSearchConfiguration
    {
        public string Uri { get; set; }
        public Dictionary<string, string> Indices  { get; set; } = 
            new Dictionary<string, string>();
        public string ProductIndex => GetIndex("Product");
        public string CategoryIndex => GetIndex("Category");
        public string ManufacturerIndex => GetIndex("Manufacturer");
        public  string ProductTagIndex => GetIndex("ProductTag");
        public  string SpecificationAttributeIndex => GetIndex("SpecificationAttribute");
        public  string ProductReviewIndex => GetIndex("ProductReview");
        public  string ImageVectorIndex => GetIndex("ImageVector");
        public  string ProductExpectedRatingIndex => GetIndex("ProductExpectedRating");


        private string GetIndex(string name)
        {
            if(Indices .TryGetValue(name, out var index))
            {
                return index;
            }

            return string.Empty;
        }
    }
}
