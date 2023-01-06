#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class SubmitOrderCommand : ICommand
    {
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();

    }
}
