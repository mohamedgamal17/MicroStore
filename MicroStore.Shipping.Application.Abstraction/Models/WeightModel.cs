using Ardalis.GuardClauses;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class WeightModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public Weight AsWeight() => Weight.FromUnit(Value, Unit);         

    }
}
