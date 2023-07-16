namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing
{
    public class PaymentRequestSearchModel : BasePagedListModel
    {
        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
