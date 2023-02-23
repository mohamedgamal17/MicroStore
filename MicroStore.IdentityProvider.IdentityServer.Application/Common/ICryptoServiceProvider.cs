namespace MicroStore.IdentityProvider.IdentityServer.Application.Common
{
    public interface ICryptoServiceProvider
    {
        Task<byte[]> GenerateRandomKey(int length);
        Task<string> GenerateRandomEncodedBase64Key(int lenght, string? perfix = null);
    }
}
