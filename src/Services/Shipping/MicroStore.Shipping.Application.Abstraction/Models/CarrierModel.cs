namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class CarrierModel
    {
        public string CarrierId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }

    
}
