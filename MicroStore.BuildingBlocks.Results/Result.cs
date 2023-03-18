namespace MicroStore.BuildingBlocks.Results
{
    public class Result<T>
    {
        private readonly T _value;

        private readonly Exception _exception;

        private readonly ResultState _state;
        public T Value => _value;
        public Exception Exception => _exception;
        public ResultState State => _state;
        public bool IsSuccess => _state == ResultState.Success;
        public bool IsFailure => !IsSuccess;

        public Result(T value)
        {
            _state = ResultState.Success;
            _value = value; 
        }
        public Result (Exception exception)
        {
            _state= ResultState.Failure;
            _exception = exception;
        }

        public R Match<R>(Func<T, R> succ, Func<Exception, R> failure)
            => IsSuccess ? succ(_value) : failure(_exception);

        public Task<R> MatchAsync<R>(Func<T, Task<R>> succ, Func<Exception, Task<R>> failure)
               => IsSuccess ? succ(_value) : failure(_exception);


        public void IfSuccess(Action<T> action)
        {
            if (IsSuccess)
            {
                action(_value);
            }
        }

        public async Task IfSuccess(Func<T,Task> action)
        {
            if (IsSuccess)
            {
                await action(_value).ConfigureAwait(false);
            }
        }

        public void IfFailure(Action<Exception> action)
        {
            if (IsFailure)
            {
                action(Exception);
            }
        }


        public async Task IfFailure(Func<T, Task> action)
        {
            if (IsFailure)
            {
                await action(_value).ConfigureAwait(false);
            }
        }


        public Result<R> Map<R>(Func<T, R> func)
            => IsFailure ? new Result<R>(_exception) : new Result<R>( func(_value));

        public async Task<Result<R>> MapAsync<R>(Func<T,Task<R>> func)=>
            IsFailure ? new Result<R>(_exception) : new Result<R>(await func(_value).ConfigureAwait(false));

        public static implicit operator Result<T>(T value) => new Result<T>(value);


        
    }


    public enum ResultState : byte
    {
        Success,
        Failure
    }

}
