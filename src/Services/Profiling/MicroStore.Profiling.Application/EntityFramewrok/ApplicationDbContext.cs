using Microsoft.EntityFrameworkCore;
using MicroStore.Profiling.Application.Domain;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Profiling.Application.EntityFramewrok
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(DbContext), IncludeDefaults = true, IncludeSelf = true)]
    public class ApplicationDbContext : AbpDbContext<ApplicationDbContext> , ITransientDependency
    {
        public DbSet<Profile> Profiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
