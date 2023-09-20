using Volo.Abp.Domain.Entities;

namespace MicroStore.Profiling.Application.Domain
{
    public class Address : Entity<string> 
    {
        public string Name { get;  set; }
        public string Phone { get;  set; }
        public string CountryCode { get;  set; }
        public string City { get;  set; }
        public string State { get;  set; }
        public string PostalCode { get;  set; }
        public string Zip { get;  set; }
        public string AddressLine1 { get;  set; }
        public string AddressLine2 { get;  set; }

        public Address()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
