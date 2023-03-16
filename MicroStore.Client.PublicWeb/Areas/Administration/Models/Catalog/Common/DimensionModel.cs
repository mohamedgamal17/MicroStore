using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Common
{
    public class DimensionModel
    {
        public double Lenght { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public DimensionUnit Unit { get; set; }
    }

}
