namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Common
{
    public class ShipEngineAddress
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        public string? AddressLine1 { get; set; }   
        public string? AddressLine2 { get; set; }   
        public string? AddressLine3 { get; set; }      
        public string? CityLocality { get; set; } 
        public string? StateProvince { get; set; }     
        public string? PostalCode { get; set; }      
        public string? CountryCode { get; set; } 
        public string? AddressResidentialIndicator { get; set; }
    }
}
