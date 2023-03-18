namespace MicroStore.BuildingBlocks.Results
{
    public class ResultV2<T>
    {
        private readonly T _value;

        private readonly Exception _exception;

        private readonly ResultState _state;
        public T Value => _value;
        public Exception Exception => _exception;
        public ResultState State => _state;
        public bool IsSuccess => _state == ResultState.Success;
        public bool IsFailure => !IsSuccess;

        public ResultV2(T value)
        {
            _state = ResultState.Success;
            _value = value; 
        }
        public ResultV2 (Exception exception)
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


        public ResultV2<R> Map<R>(Func<T, R> func)
            => IsFailure ? new ResultV2<R>(_exception) : new ResultV2<R>( func(_value));

        public async Task<ResultV2<R>> MapAsync<R>(Func<T,Task<R>> func)=>
            IsFailure ? new ResultV2<R>(_exception) : new ResultV2<R>(await func(_value).ConfigureAwait(false));

        public static implicit operator ResultV2<T>(T value) => new ResultV2<T>(value);


        
    }


    public enum ResultState : byte
    {
        Success,
        Failure
    }

}
