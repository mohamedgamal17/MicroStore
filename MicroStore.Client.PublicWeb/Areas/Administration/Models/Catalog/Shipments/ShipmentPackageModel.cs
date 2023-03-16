using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Shipments
{
    public class ShipmentPackageModel
    {
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }

    }
}
