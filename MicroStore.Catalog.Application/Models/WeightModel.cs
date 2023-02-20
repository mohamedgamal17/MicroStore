#pragma warning disable CS8618
using MicroStore.Catalog.Domain.ValueObjects;
namespace MicroStore.Catalog.Application.Models
{
    public class WeightModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public Weight AsWeight() => Weight.FromUnit(Value, Unit);
    }
}
