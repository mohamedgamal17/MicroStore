using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShippingSystemDto 
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
