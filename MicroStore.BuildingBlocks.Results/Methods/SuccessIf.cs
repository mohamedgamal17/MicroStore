

namespace MicroStore.BuildingBlocks.Results
{
    public partial class Result
    {
        public static Result SuccessIf(bool isSuccess, string error)
        {
            return isSuccess ? Success() : Failure(error);
        }
        public static Result<TValue> SuccessIf<TValue>(bool isSuccess, TValue value, string error)
        {
            return isSuccess ? Success(value) : Failure<TValue>(error);
        }

        public static Result SuccessIf(Func<bool> predicate, string error)
        {
            bool isSuccess = predicate();
            return SuccessIf(isSuccess, error);
        }

        public static Result<TValue> SuccessIf<TValue>(Func<bool> predicate, TValue value, string error)
        {
            bool isSuccess = predicate();
            return SuccessIf(isSuccess, value, error);
        }

        public async static Task<Result> SuccessIfAsync(Func<Task<bool>> predicate, string error)
        {
            bool isSuccess = await predicate().ConfigureAwait(false);
            return SuccessIf(isSuccess, error);
        }

        public async static Task<Result<TValue>> SuccessIfAsync<TValue>(Func<Task<bool>> predicate, TValue value, string error)
        {
            bool isSuccess = await predicate().ConfigureAwait(false);
            return SuccessIf(isSuccess, value, error);

        }
    }
}
