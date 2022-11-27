
namespace MicroStore.BuildingBlocks.Results
{
    public partial class Result : IResult
    {

        private bool _isSucess;
        private object? _error;
        public bool IsSuccess => _isSucess;
        public bool IsFailure => !_isSucess;

        public object Error
        {
            get
            {
                if (IsFailure)
                {
                    return _error!;
                }

                throw new InvalidOperationException("result is already succeeded");
            }

        }


        internal Result(bool isSucess, object? error)
        {
            _isSucess = isSucess;
            _error = error;
        }


        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(true, value, string.Empty);
        }

        public static Result Failure(object error)
        {
            return new Result(false, error);
        }

        public static Result<TValue> Failure<TValue>(object error)
        {
            return new Result<TValue>(false, default(TValue), error);
        }

        public static ResponseResult Success(string code)
        {
            return new ResponseResult(true, code, string.Empty);
        }


        public static ResponseResult Failure(object error, string code)
        {
            return new ResponseResult(false, code, error);
        }

        public static ResponseResult<T> Success<T>(T value, string code)
        {
            return new ResponseResult<T>(true, value, code, string.Empty);
        }

        public static ResponseResult<T> Failure<T>(object error, string code)
        {
            return new ResponseResult<T>(false, default(T), code, error);
        }
    }


    public class Result<TValue> : Result, IResult<TValue>
    {
        private TValue? _value;

        public TValue Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("result is already failured");
                }

                return _value!;
            }
        }
        internal Result(bool isSucess, TValue? value, object? error) :
            base(isSucess, error)
        {
            _value = value;
        }

    }
}
