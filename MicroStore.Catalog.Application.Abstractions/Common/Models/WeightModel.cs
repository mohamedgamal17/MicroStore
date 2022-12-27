using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Application.Abstractions.Common.Models
{
    public class WeightModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public Weight AsWeight() => Weight.FromUnit(Value, Unit);
    }
}
