
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using Volo.Abp.DependencyInjection;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public class RequestHandlerConventionalRegistar : DefaultConventionalRegistrar
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


        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }


    public class RequestHandlerConventionalRegistarV2 : DefaultConventionalRegistrar
    {
        protected override bool IsConventionalRegistrationDisabled(Type type)
        {
            return !CanRegister(type);
        }


        protected override List<Type> GetExposedServiceTypes(Type type)
        {
            return type.GetInterfaces()
                .Where(x => x.IsGenericType)
                .Where(x => x.GetGenericTypeDefinition() == typeof(IRequestHandlerV2<,>)).ToList();

        }

        private bool CanRegister(Type type)
        {
            return IsRequestHandler(type);
        }

        private bool IsRequestHandler(Type type)
            => type.GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRequestHandlerV2<,>));


        protected override ServiceLifetime? GetDefaultLifeTimeOrNull(Type type)
        {
            return ServiceLifetime.Transient;
        }
    }
}
