
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class UpdateShippingSettingsModel
    {
        public string DefaultShippingSystem { get; set; } 

        public AddressModel Location { get; set; }
    }
}
