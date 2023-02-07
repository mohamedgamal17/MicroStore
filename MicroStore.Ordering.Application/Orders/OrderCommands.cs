using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
namespace MicroStore.Ordering.Application.Orders
{
    public class SubmitOrderCommand : ICommand<OrderSubmitedDto>
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
    public class FullfillOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
    }
    public class CompleteOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public DateTime ShipedDate { get; set; }
    }


    public class CancelOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
