namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IWeightSystemResolver
    {
        Task<IWeightSytemProvider> Resolve(CancellationToken cancellationToken =default);

    }
}
