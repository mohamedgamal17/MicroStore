namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public interface IUIExceptionHandler<TException>
    {
        Task HandleAsync(HttpContext context , TException exception);
    }



}
