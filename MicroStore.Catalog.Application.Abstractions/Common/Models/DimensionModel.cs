using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Application.Abstractions.Common.Models
{
    public class DimensionModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }

        public Dimension AsDimension()
        {
            return Unit.ToLower() switch
            {
               "cm" => Dimension.FromCentiMeter(Value),
               "m" => Dimension.FromMeter(Value),
               "inch"=> Dimension.FromInc(Value),
               "ft" => Dimension.FromFeet(Value),
               _ => Dimension.Empty
            };
        }
    }
}
