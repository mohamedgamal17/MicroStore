using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Common
{
    public interface IInventoyDbContext
    {
        DbSet<Product> Products { get; set; }
    }
}
