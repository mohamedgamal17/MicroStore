using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderVM
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]      
        public OrderState CurrentState { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemVM> Items { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
    }
}
