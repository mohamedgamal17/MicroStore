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



        protected async Task<Country> CreateCountry()
        {
            Country country = new Country
            {
                Name = Guid.NewGuid().ToString(),
                NumericIsoCode = new Random().Next(0, 200),
                TwoLetterIsoCode = RandomString(2),
                ThreeLetterIsoCode = RandomString(3),
            };


            return await Insert(country);
        }

        protected async Task<Country> CreateCountryWithStateProvince()
        {
            Country country = new Country
            {
                Name = Guid.NewGuid().ToString(),
                NumericIsoCode = new Random().Next(0,200),
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



            return await Insert(country);
        }
    }




}
