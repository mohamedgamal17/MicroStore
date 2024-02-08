namespace MicroStore.Bff.Shopping.Models.Geographic
{
    public class StateProvinceModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public StateProvinceModel()
        {
            Name = string.Empty;
            Abbreviation = string.Empty;
        }
    }
}
