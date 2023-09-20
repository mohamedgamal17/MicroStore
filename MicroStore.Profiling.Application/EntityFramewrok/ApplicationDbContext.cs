using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Profiling.Application.EntityFramewrok
{
    public class ApplicationDbContext : AbpDbContext<ApplicationDbContext>
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
