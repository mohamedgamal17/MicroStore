using IdentityModel;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Common.Models
{
    public class SecretModel
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public HasingAlgoritm Algoritm { get; set; }

        public string ResolveApiResourceSecret()
        {
            if (Algoritm == HasingAlgoritm.SHA256)
            {
                return Value.ToSha256();
            }
            else
            {
                return Value.ToSha512();
            }

        }
    }


    public enum HasingAlgoritm
    {
        SHA256 = 0,
        SHA512 = 5,
    }

}
