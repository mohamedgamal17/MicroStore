using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.WebApi.Models
{
    public class UpdateShippingSettingsModel
    {
        public string DefaultShippingSystem { get; set; }
        public AddressModel Location { get; set; }
    }
}
