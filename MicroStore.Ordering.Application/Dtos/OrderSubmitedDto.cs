#nullable disable
using MicroStore;
using MicroStore.Ordering.Application.Models;
namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderSubmitedDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();


    }
}
