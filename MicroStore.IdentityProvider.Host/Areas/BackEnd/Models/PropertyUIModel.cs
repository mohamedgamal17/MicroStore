namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models
{
    public class PropertyUIModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }


    public class RemovePropertyUIModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }
    }
}
