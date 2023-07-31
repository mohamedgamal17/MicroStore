namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class CreateOrEditClientClaimModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class RemoveClientClaimModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }
    }
}
