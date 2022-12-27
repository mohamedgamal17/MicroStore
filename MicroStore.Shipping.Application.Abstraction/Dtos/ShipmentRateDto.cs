namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentRateDto
    {
        public string RateId { get; set; }
        public string CarrierId { get; set; }
        public MoneyDto Amount { get; set; }
        public ServiceLevelDto ServiceLevel { get; set; }
        public int?  Days { get; set; }

        [Obsolete]
        public string DurationTerms { get; set; }
    }
}
    