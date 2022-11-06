

namespace MicroStore.BuildingBlocks.Results
{
    public partial class Result
    {
        public static Result FailureIf(bool isFailure, string error)
        {
            return isFailure ? Failure(error) : Success(error);
        }
        public static Result<TValue> FailureIf<TValue>(bool isFailure, TValue value, string error)
        {
            return isFailure ? Failure<TValue>(error) : Success(value);
        }

        public static Result FailureIf(Func<bool> predicate, string error)
        {
            bool isFailure = predicate();
            return FailureIf(isFailure, error);
        }

        public static Result<TValue> FailureIf<TValue>(Func<bool> predicate, TValue value, string error)
        {
            bool isFailure = predicate();
            return FailureIf(isFailure, value, error);
        }

        public async static Task<Result> FailureIfAsync(Func<Task<bool>> predicate, string error)
        {
            bool isFailure = await predicate().ConfigureAwait(false);
            return FailureIf(isFailure, error);
        }

        public async static Task<Result<TValue>> FailureIfAsync<TValue>(Func<Task<bool>> predicate, TValue value, string error)
        {
            bool isFailure = await predicate().ConfigureAwait(false);
            return FailureIf(isFailure, value, error);

        }

    }
}
