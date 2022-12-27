using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Common;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Shipping.Infrastructure
{
    public class UnitSystemProviderConventionalRegistar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !CanRegister(type);
        }


        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(x => x == typeof(IWeightSytemProvider) || x == typeof(IDimensionSystemProvider)).ToList();
        }

        private bool CanRegister(Type type)
        {
            return type.GetInterfaces()
                .Any(x => x == typeof(IWeightSytemProvider) || x == typeof(IDimensionSystemProvider));
        }
        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
