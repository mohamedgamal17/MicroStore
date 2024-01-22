namespace MicroStore.Bff.Shopping.Data.Geographic
{
    public class Country : AuditiedEntity <string>
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvince> StateProvinces { get; set; }
    }
}
