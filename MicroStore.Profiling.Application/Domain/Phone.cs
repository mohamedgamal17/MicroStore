using PhoneNumbers;
using System;

namespace MicroStore.Profiling.Application.Domain
{
    public class Phone : ValueObject<Phone>
    {

        private string _number;
        public string Number=> _number;
        private Phone() { }
        private Phone(string number)
        {            
            var phoneNumberParsed = ParsePhoneNumber(number);

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            _number = phoneNumberUtil.Format(phoneNumberParsed,PhoneNumberFormat.E164);
        }

        private PhoneNumber ParsePhoneNumber(string phone)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            var phoneNumberParsed = phoneNumberUtil.Parse(phone,null);

            if(!phoneNumberUtil.IsValidNumber(phoneNumberParsed))
            {
                throw new InvalidOperationException("Invalid phone number");
            }

            return phoneNumberParsed;
        }

        public static Phone Create(string number) => new Phone(number);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _number;
                
        }
    }
}
