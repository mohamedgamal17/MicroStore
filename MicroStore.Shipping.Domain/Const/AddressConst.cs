namespace MicroStore.Shipping.Domain.Const
{
    public class AddressConst
    {
        public static string Name => "Address_CustomerName";
        public static string Phone => "Address_CustomerPhone";
        public static string CountryCode => "Address_CountryCode";
        public static string City => "Address_City";
        public static string State => "Address_State";
        public static string PostalCode => "Address_PostalCode";
        public static string Zip => "Address_Zip";
        public static string AddressLine1 => "Address_AddressLine1";
        public static string AddressLine2 => "Address_AddressLine2";

        public static int NameMaxLength => 265;
        public static int PhoneMaxLength => 265;
        public static int CountryCodeMaxLength => 50;
        public static int CityMaxLength => 100;
        public static int StateMaxLength => 100;
        public static int PostalCodeMaxLength => 50;
        public static int ZipMaxLength => 50;
        public static int AddressLineMaxLenght => 350;

    }
}
