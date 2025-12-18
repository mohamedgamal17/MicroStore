using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
namespace MicroStore.IdentityProvider.Identity.Application.Common
{
    public interface IApplicationIdentityDbContext
    {
        DbSet<ApplicationIdentityUser> Users { get; set; }
        DbSet<ApplicationIdentityRole> Roles { get; set; }

    }
}
