using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class CreateOrderShipmmentModel
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}
