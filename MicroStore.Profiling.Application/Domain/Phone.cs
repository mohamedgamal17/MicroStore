using PhoneNumbers;
using System;

namespace MicroStore.Profiling.Application.Domain
{
    public class Phone : ValueObject<Phone>
    {

        private string _number;

        private int _countryCode;
        public string Number=> _number;
        public int CountryCode => _countryCode;
        private Phone() { }

        private Phone(string number, string countryCode)
        {
            
            var phoneNumberParsed = ParsePhoneNumber(number, countryCode);
            _number = number;
            _countryCode = phoneNumberParsed.CountryCode;
        }

        private PhoneNumber ParsePhoneNumber(string phone, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var phoneNumberParsed = phoneNumberUtil.Parse(phone, countryCode);

            if(!phoneNumberUtil.IsPossibleNumberForType(phoneNumberParsed, PhoneNumberType.MOBILE))
            {
                throw new InvalidOperationException("Invalid phone number");
            }

            return phoneNumberParsed;
        }

        public static Phone Create(string number, string countryCode) => new Phone(number, countryCode);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _number;
            yield return _countryCode;
                
        }
    }
}
