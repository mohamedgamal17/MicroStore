using Microsoft.EntityFrameworkCore;
using MicroStore.ShoppingCart.Domain.Entities;
namespace MicroStore.ShoppingCart.Application.Abstraction
{
    public interface  IBasketDbContext
    {
        DbSet<Basket> Baskets { get; set; }
        DbSet<Product> Products { get; set; }
    }
}
