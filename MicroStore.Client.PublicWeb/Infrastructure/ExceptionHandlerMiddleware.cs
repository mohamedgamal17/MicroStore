namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;


        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }catch(Exception exception)
            {
                var execptionHandlerType = typeof(IUIExceptionHandler<>).MakeGenericType(exception.GetType());

                var exceptionHandler = context.RequestServices.GetService(execptionHandlerType);

                if (exceptionHandler != null)
                {
                    var methodInfo = execptionHandlerType.GetMethod("HandleAsync");

                    await (Task)methodInfo!.Invoke(exceptionHandler, new object[] { context, exception })!;
                }
                else
                {
                    LogException(exception);

                    context.Response.Redirect("/Error");
                }
            }
        }

        private void LogException(Exception exception)
        {
            _logger.LogException(exception);
        }
    }
}
