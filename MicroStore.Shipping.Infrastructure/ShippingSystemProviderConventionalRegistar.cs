using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using Volo.Abp.AutoMapper;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Shipping.Infrastructure
{
    public class ShippingSystemProviderConventionalRegistar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !CanRegister(type);
        }


        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(x => x == typeof(IShipmentSystemProvider)).ToList();
        }

        private bool CanRegister(Type type)
        {
            return type.GetInterfaces()
                .Any(x => x == typeof(IShipmentSystemProvider));
        }
        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }



    }
}
