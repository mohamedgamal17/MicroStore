using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Entities.ElasticSearch.Common
{
    public class ElasticDimension
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public DimensionUnit Unit { get; set; } 
    }
}
