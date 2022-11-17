

namespace MicroStore.BuildingBlocks.Results
{
    public partial class Result
    {
        public static Result FailureIf(bool isFailure, object error)
        {
            return isFailure ? Failure(error) : Success(error);
        }
        public static Result<TValue> FailureIf<TValue>(bool isFailure, TValue value, object error)
        {
            return isFailure ? Failure<TValue>(error) : Success(value);
        }

        public static Result FailureIf(Func<bool> predicate, object error)
        {
            bool isFailure = predicate();
            return FailureIf(isFailure, error);
        }

        public static Result<TValue> FailureIf<TValue>(Func<bool> predicate, TValue value, object error)
        {
            bool isFailure = predicate();
            return FailureIf(isFailure, value, error);
        }

        public async static Task<Result> FailureIfAsync(Func<Task<bool>> predicate, object error)
        {
            bool isFailure = await predicate().ConfigureAwait(false);
            return FailureIf(isFailure, error);
        }

        public async static Task<Result<TValue>> FailureIfAsync<TValue>(Func<Task<bool>> predicate, TValue value, object error)
        {
            bool isFailure = await predicate().ConfigureAwait(false);
            return FailureIf(isFailure, value, error);

        }

    }
}
