using MicroStore.Inventory.Domain.ProductAggregate;
using System.Linq.Expressions;

namespace MicroStore.Inventory.Application.Common
{
    public interface IProductRepository
    {
        Task<Product?> SingleOrDefaultAsync(Expression<Func<Product, bool>> expression, CancellationToken cancellationToken = default);
        Task AddOrUpdateAsync(Product product, CancellationToken cancellationToken = default);
    }
}
