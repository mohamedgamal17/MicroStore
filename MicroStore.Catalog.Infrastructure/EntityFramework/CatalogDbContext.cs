﻿using Microsoft.EntityFrameworkCore;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Domain.Entities;
using System.Reflection;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace MicroStore.Catalog.Infrastructure.EntityFramework
{
    [ConnectionStringName("DefaultConnection")]
    [ExposeServices(typeof(ICatalogDbContext), IncludeSelf = true)]
    public class CatalogDbContext : AbpDbContext<CatalogDbContext>, ICatalogDbContext ,ITransientDependency
    {

        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; } 

        public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions)
        : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }


    }
}
