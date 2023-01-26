using Microsoft.EntityFrameworkCore;
using MicroStore.IdentityProvider.Identity.Application.Domain;

namespace MicroStore.IdentityProvider.Identity.Application.Common.Interfaces
{
    public interface IApplicationIdentityDbContext
    {
         DbSet<ApplicationIdentityUser> Users { get; set; }
         DbSet<ApplicationIdentityRole> Roles { get; set; }

    }
}
