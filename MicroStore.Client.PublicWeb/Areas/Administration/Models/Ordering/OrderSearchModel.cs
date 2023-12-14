using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderSearchModel : BasePagedListModel
    {
        public string? OrderNumber { get; set; }

        public OrderState[]? States { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartSubmissionDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndSubmissionDate { get; set; }
    }


   
}
