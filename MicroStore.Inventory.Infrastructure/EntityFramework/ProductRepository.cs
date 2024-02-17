using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Linq.Expressions;

namespace MicroStore.Inventory.Infrastructure.EntityFramework
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext context;

        public ProductRepository(InventoryDbContext inventoryDbContext)
        {
            context = inventoryDbContext;
        }

        public async Task<Product?> SingleOrDefaultAsync(Expression<Func<Product, bool>> expression,  CancellationToken cancellationToken  = default)
        {
            return await context.Products.SingleOrDefaultAsync(expression, cancellationToken);
        }

        public async Task AddOrUpdateAsync(Product product , CancellationToken cancellationToken  = default )
        {

            if (context.Products.Any(e => e.Id == product.Id))
            {
                context.Products.Add(product);

                context.Products.Attach(product);
            }
            else
            {
                context.Products.Add(product);
            }


            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
