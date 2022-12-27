namespace MicroStore.Payment.Application.Abstractions.Dtos
{
    public class PaymentSystemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
