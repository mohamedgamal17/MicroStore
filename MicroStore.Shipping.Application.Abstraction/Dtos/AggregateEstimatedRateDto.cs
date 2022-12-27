namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class AggregateEstimatedRateDto
    {
        public string  SystemName { get; set; }
        public List<EstimatedRateDto> Rates { get; set; }
    }
    public class AggregateEstimatedRateCollection : List<AggregateEstimatedRateDto> { }
}
