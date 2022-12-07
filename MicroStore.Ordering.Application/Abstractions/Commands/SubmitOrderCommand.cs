#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class SubmitOrderCommand : ICommand<OrderSubmitedDto>
    {
        public Guid ShippingAddressId { get; set; }
        public Guid BillingAddressId { get; set; }
        public string UserId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();

    }
}
