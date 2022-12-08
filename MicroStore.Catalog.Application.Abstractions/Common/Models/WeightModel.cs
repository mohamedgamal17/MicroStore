using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Application.Abstractions.Common.Models
{
    public class WeightModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public Weight AsWeight()
        {
            return Unit.ToLower() switch
            {
                "g"   => Weight.FromGram(Value),
                "kg"  => Weight.FromKiloGram(Value),
                "lb"  => Weight.FromPound(Value),
                _ => Weight.Empty
            };
        }
    }
}
