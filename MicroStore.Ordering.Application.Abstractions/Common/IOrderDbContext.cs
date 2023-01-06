using Microsoft.EntityFrameworkCore;

namespace MicroStore.Ordering.Application.Abstractions.Common
{
    public interface IOrderDbContext
    {
        DbSet<TEntity> Query<TEntity>() where TEntity : class;
    }
}
