namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class RemovePropertyModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }
    }
}
