using Microsoft.EntityFrameworkCore;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ProductAggregate;

namespace MicroStore.Inventory.Application.Common
{
    public interface IInventoyDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
    }
}
