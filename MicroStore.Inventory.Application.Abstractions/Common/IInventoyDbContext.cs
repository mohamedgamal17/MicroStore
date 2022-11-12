using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public interface IInventoyDbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
