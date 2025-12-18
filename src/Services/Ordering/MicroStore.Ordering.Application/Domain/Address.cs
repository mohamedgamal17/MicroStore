namespace MicroStore.Ordering.Application.Domain
{
    public class Address : ValueObject<Address>
    {
        public static readonly Address Empty = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        public string Name { get; protected set; }
        public string Phone { get; protected set; }
        public string CountryCode { get; protected set; }
        public string City { get; protected set; }
        public string State { get; protected set; }
        public string PostalCode { get; protected set; }
        public string Zip { get; protected set; }
        public string AddressLine1 { get; protected set; }
        public string AddressLine2 { get; protected set; }

        protected Address() { }



        internal Address(string name, string phone, string countryCode, string city, string state, string postalCode, string zip, string addressLine1, string addressLine2)
        {
            Name = name;
            Phone = phone;
            CountryCode = countryCode;
            City = city;
            State = state;
            PostalCode = postalCode;
            Zip = zip;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Phone;
            yield return CountryCode;
            yield return City;
            yield return State;
            yield return PostalCode;
            yield return Zip;
            yield return AddressLine1;
            yield return AddressLine2;
        }
    }


    public class AddressBuilder
    {
        protected string Name { get; private set; } = string.Empty;
        protected string Phone { get; set; } = string.Empty;
        protected string CountryCode { get; set; } = string.Empty;
        protected string City { get; set; } = string.Empty;
        protected string State { get; set; } = string.Empty;
        protected string PostalCode { get; set; } = string.Empty;
        protected string Zip { get; set; } = string.Empty;
        protected string AddressLine1 { get; set; } = string.Empty;
        protected string AddressLine2 { get; set; } = string.Empty;


        public AddressBuilder WithName(string name)
        {
            Name = name;
            return this;
        }
        public AddressBuilder WithPhone(string phone)
        {
            Phone = phone;
            return this;
        }
        public AddressBuilder WithCountryCode(string countryCode)
        {
            CountryCode = countryCode;
            return this;
        }
        public AddressBuilder WithCity(string city)
        {
            City = city;
            return this;
        }
        public AddressBuilder WithState(string state)
        {
            State = state;
            return this;
        }
        public AddressBuilder WithPostalCode(string postalCode)
        {
            PostalCode = postalCode;
            return this;
        }
        public AddressBuilder WithZip(string zip)
        {
            Zip = zip;
            return this;
        }
        public AddressBuilder WithAddressLine(string addressLine1, string addressLine2)
        {
            AddressLine1 = addressLine1; ;
            AddressLine2 = addressLine2;
            return this;
        }
        public Address Build()
        {
            return new Address(Name, Phone, CountryCode, City, State, PostalCode, Zip, AddressLine1, AddressLine2);
        }
    }
}
