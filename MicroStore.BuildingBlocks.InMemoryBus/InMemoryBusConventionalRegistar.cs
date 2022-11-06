
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using Volo.Abp.DependencyInjection;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public class InMemoryBusConventionalRegistar : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !CanRegister(type);
        }


        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(x => x.IsGenericType)
                .Where(x => x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToList();


        }

        private bool CanRegister(Type type)
        {
            return IsRequestHandler(type);
        }

        private bool IsRequestHandler(Type type)
            => type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

        private bool IsRequestMiddleware(Type type)
            => type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestMiddleware<,>));


        private bool IsRequestPreProcess(Type type)
            => type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestPreProcessor<>));


        private bool IsRequestPostProcess(Type type)
            => type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestPostProcess<,>));

        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
