

namespace MicroStore.BuildingBlocks.Results
{
    public partial class Result
    {
        public static Result Try(Action action, Func<Exception, object> errorHandler)
        {
            try
            {
                action();
                return Success();
            }
            catch (Exception ex)
            {
                object error = errorHandler(ex);

                return Failure(error);
            }
        }

        public static async Task<Result> TryAsync(Func<Task> func, Func<Exception, object> errorHandler)
        {
            try
            {
                await func().ConfigureAwait(false);

                return Result.Success();
            }
            catch (Exception ex)
            {
                object error = errorHandler(ex);

                return Failure(error);
            }
        }

        public static Result<T> TryAsync<T>(Func<T> func, Func<Exception, object> errorHandler)
        {
            try
            {
                T result = func();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                object error = errorHandler(ex);

                return Failure<T>(error);
            }
        }

        public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<Exception, object> errorHandler)
        {
            try
            {
                T result = await func().ConfigureAwait(false);
                return Success(result);
            }
            catch (Exception ex)
            {
                object error = errorHandler(ex);

                return Failure<T>(error);
            }
        }
    }
}
