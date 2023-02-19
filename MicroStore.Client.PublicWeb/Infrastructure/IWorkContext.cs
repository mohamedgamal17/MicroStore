namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public interface IWorkContext
    {
        string TryToGetCurrentUserId();
    }
}
