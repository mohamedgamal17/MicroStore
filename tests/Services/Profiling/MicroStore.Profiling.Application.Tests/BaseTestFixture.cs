using MicroStore.Profiling.Application.Domain;
using MicroStore.TestBase;

namespace MicroStore.Profiling.Application.Tests
{
    [TestFixture]
    public class BaseTestFixture :  ApplicationTestBase<ProfilingApplicationTestModule>
    {


        public async Task<Profile> CreateFakeProfile()
        {
            var profile = new Profile
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Gender = Gender.Male,
                BirthDate = DateTime.Now.AddYears(-18),
                UserId = Guid.NewGuid().ToString(),
                Phone = Phone.Create("+18143511258"),
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Name = Guid.NewGuid().ToString(),
                        CountryCode = "US",
                        State = "TX",
                        City = "Austin",
                        Phone = "+18143511258",
                        PostalCode = "5461",
                        AddressLine1= Guid.NewGuid().ToString(),
                        AddressLine2= Guid.NewGuid().ToString(),
                        Zip = "6321"
                    }
                }
            };

            await Insert(profile);

            return profile;
        }
    }
}
