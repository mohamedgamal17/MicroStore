using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderSearchModel : BasePagedListModel
    {
        public string? OrderNumber { get; set; }

        public OrderState[] States { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartSubmissionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndSubmissionDate { get; set; }
    }


   
}
