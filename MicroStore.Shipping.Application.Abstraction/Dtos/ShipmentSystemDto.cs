namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentSystemDto
    {
        public Guid SystemId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
