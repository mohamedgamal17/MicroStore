using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.Clients
{
    public class CreateOrEditClientClaimModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }

        [MaxLength(200)]
        public string Type { get; set; }

        [MaxLength(200)]
        public string Value { get; set; }
    }

    public class RemoveClientClaimModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }
    }
}
