using MicroStore.Bff.Shopping.Data.Common;

namespace MicroStore.Bff.Shopping.Models.Common
{
    public class DimensionModel
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public DimensionUnit Unit { get; set; }
    }
}
