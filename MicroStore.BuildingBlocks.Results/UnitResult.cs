using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.BuildingBlocks.Results
{
    public class UnitResult
    {
        private bool _isSuccess;
        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;
        public ErrorInfo Error { get; }


        internal UnitResult(bool isSuccess , ErrorInfo error = null)
        {
            _isSuccess = isSuccess;
            Error = error;
        }



        public static UnitResult Success()
        {
            return new UnitResult(true, ErrorInfo.Empty);
        }
        public static UnitResult Failure(ErrorInfo error)
        {
            return new UnitResult(false, error);
        }

        public static UnitResult<T> Success<T> (T value)
        {
            return new UnitResult<T>(true, value, ErrorInfo.Empty);
        }

        public static UnitResult<T> Failure<T>(ErrorInfo error)
        {
            return new UnitResult<T>(false, error: error);
        }



    }



    public class UnitResult<T> : UnitResult
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException(Error.Message);
                }

                return _value;
            }
        }

        internal UnitResult(bool isSuccess,  T value = default(T) , ErrorInfo error = null)
            : base(isSuccess,error)
        {
            _value = value;
        }
    }
}
