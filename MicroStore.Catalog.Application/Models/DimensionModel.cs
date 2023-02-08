using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Application.Models
{
    public class DimensionModel
    {
        public double Width { get; set; }
        public double Lenght { get; set; }
        public double Height { get; set; }
        public string Unit { get; set; }

        public Dimension AsDimension() => Dimension.FromUnit(Width, Lenght, Height, Unit);
    }
}
