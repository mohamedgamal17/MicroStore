using MicroStore.Shipping.Application.Abstraction.Common;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Shipping.Infrastructure
{
    public class DimensionSystemResolver : IDimensionSystemResolver, ISingletonDependency
    {
        private readonly IList<IDimensionSystemProvider> _providers;

        private readonly string _defaultSystemName = "NAN";

        public DimensionSystemResolver(IList<IDimensionSystemProvider> providers)
        {
            _providers = providers;
        }

        public Task<IDimensionSystemProvider> Resolve(CancellationToken cancellationToken = default)
        {
            var system = _providers.SingleOrDefault(x => x.SystemName == _defaultSystemName);

            if (system == null)
            {
                system = _providers.First();
            }

            return Task.FromResult(system);
        }
    }
}
