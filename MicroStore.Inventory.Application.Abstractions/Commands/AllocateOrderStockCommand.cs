#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Common;
namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class AllocateOrderStockCommand : ICommand
    {
        public string ExternalOrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string ExternalPaymentId { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddres { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}
