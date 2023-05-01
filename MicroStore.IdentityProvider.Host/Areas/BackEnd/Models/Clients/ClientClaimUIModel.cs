namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients
{
    public class ClientClaimUIModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }

    public class RemoveClientClaimUIModel
    {
        public int ClientId { get; set; }

        public int ClaimId { get; set; }
    }
}
