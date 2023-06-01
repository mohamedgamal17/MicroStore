using Microsoft.EntityFrameworkCore;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Common
{
    public interface ICatalogDbContext
    {
        DbSet<Product> Products { get; set; }

        DbSet<Category> Categories { get; set; }

        DbSet<Manufacturer> Manufacturers { get; set; }

        DbSet<ProductReview> ProductReviews { get; set; }
    }
}
