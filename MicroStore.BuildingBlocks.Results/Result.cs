
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
