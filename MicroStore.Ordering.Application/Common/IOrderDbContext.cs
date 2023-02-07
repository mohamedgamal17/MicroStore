using Microsoft.EntityFrameworkCore;

namespace MicroStore.Ordering.Application.Common
{
    public interface IOrderDbContext
    {
        DbSet<TEntity> Query<TEntity>() where TEntity : class;
    }
}
