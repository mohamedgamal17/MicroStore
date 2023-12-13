using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing
{
    public class PaymentRequestSearchModel : BasePagedListModel
    {
        public string? OrderNumber { get; set; }
        public PaymentStatus? Status { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
    }
}
