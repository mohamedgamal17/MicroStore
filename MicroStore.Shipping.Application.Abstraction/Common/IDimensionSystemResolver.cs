namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IDimensionSystemResolver
    {
        Task<IDimensionSystemProvider> Resolve(CancellationToken cancellationToken = default);

    }
}
