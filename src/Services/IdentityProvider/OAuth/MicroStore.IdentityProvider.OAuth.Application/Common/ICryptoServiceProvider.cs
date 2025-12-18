namespace MicroStore.IdentityProvider.OAuth.Application.Common
{
    public interface ICryptoServiceProvider
    {
        Task<byte[]> GenerateRandomKey(int length);
        Task<string> GenerateRandomEncodedBase64Key(int lenght, string? perfix = null);
    }
}
