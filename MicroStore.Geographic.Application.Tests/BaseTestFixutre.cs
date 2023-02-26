using Microsoft.Extensions.DependencyInjection;
using MicroStore.Geographic.Application.EntityFramework;
using MicroStore.TestBase;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MicroStore.Geographic.Application.Domain;

namespace MicroStore.Geographic.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixutre : ApplicationTestBase<GeographicApplicationTestModule>
    {
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();

            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }


        public async Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] properties) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

            var query = dbContext.Set<TEntity>().AsQueryable();

            foreach (var prop in properties)
            {
                query = query.Include(prop);
            }
            return await dbContext.Set<TEntity>().SingleAsync(expression);
        }

        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

            await dbContext.AddAsync(entity);

            await dbContext.SaveChangesAsync();

            return entity;

        }

        public async Task<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

            dbContext.Attach(entity);

            dbContext.Update(entity);

            await dbContext.SaveChangesAsync();

            return entity;

        }

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();
            return await dbContext.Set<TEntity>().SingleAsync(expression);

        }

        public async Task<TEntity?> SingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();
            return await dbContext.Set<TEntity>().SingleOrDefaultAsync(expression);

        }

        public async Task<TEntity> FirstAsync<TEntity>() where TEntity : class
        {
            using var scope = ServiceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

            return await dbContext.Set<TEntity>().FirstAsync();
        }

        protected async Task<Country> CreateCountry()
        {
            Country country = new Country
            {
                Name = Guid.NewGuid().ToString(),
                NumericIsoCode = 22,
                TwoLetterIsoCode = RandomString(2),
                ThreeLetterIsoCode = RandomString(3),
            };


            return await InsertAsync(country);
        }

        protected async Task<Country> CreateCountryWithStateProvince()
        {
            Country country = new Country
            {
                Name = Guid.NewGuid().ToString(),
                NumericIsoCode = 22,
                TwoLetterIsoCode = RandomString(2),
                ThreeLetterIsoCode = RandomString(3),
            };

            country.StateProvinces.Add(new StateProvince
            {
                Name = Guid.NewGuid().ToString(),
                Abbreviation = RandomString(2),
            });


            country.StateProvinces.Add(new StateProvince
            {
                Name = Guid.NewGuid().ToString(),
                Abbreviation = RandomString(2),


            });

            country.StateProvinces.Add(new StateProvince
            {
                Name = Guid.NewGuid().ToString(),
                Abbreviation = RandomString(2),


            });



            return await InsertAsync(country);
        }
    }




}
