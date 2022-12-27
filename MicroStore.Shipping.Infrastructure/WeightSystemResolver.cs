using MicroStore.Shipping.Application.Abstraction.Common;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Shipping.Infrastructure
{
    public class WeightSystemResolver : IWeightSystemResolver, ISingletonDependency
    {
        private readonly IList<IWeightSytemProvider> _providers;

        private string _defaultWeightSystem = "NAN"; // hard coded for now 

        public WeightSystemResolver(IList<IWeightSytemProvider> providers)
        {
            _providers = providers;
        }

        public Task<IWeightSytemProvider> Resolve(CancellationToken cancellationToken = default)
        {
            var system = _providers.SingleOrDefault(x => x.SystemName == _defaultWeightSystem);

            if (system == null)
            {
                system = _providers.First();
            }


            return Task.FromResult(system);
        }
    }
}
