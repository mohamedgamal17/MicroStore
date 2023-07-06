using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentPackageModel
    {
        public string Id { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }

    }
}
