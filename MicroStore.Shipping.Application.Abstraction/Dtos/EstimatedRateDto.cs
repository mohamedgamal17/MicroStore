namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class EstimatedRateDto
    {
        public string Name { get; set; }
        public MoneyDto Money { get; set; }
        public int EstaimatedDays { get; set; }
        public DateTime? ShippingDate { get; set; }
    }
}
