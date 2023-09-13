using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Entities.ElasticSearch.Common
{
    public class ElasticWeight
    {
        public double Value { get; set; }
        public WeightUnit Unit { get; set; }
    }
}
