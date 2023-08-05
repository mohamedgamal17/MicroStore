using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models
{
    public class PropertyViewModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }

        [MaxLength(200)]
        [Required]
        public string Key { get; set; }

        [MaxLength(200)]
        [Required]
        public string Value { get; set; }
    }

    public class RemovePropertyModel
    {
        public int PropertyId { get; set; }
        public int ParentId { get; set; }
    }
}
