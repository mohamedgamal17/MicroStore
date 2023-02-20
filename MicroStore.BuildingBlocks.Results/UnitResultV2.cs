using MicroStore.BuildingBlocks.Results.Http;

namespace MicroStore.BuildingBlocks.Results
{
    public class UnitResultV2
    {
        private bool _isSuccess;
        public bool IsSuccess => _isSuccess;
        public bool IsFailure => !_isSuccess;
        public ErrorInfo Error { get; }


        internal UnitResultV2(bool isSuccess , ErrorInfo error = null)
        {
            _isSuccess = isSuccess;
            Error = error;
        }



        public static UnitResultV2 Success()
        {
            return new UnitResultV2(true);
        }
        public static UnitResultV2 Failure(ErrorInfo error)
        {
            return new UnitResultV2(false, error);
        }

        public static UnitResultV2<T> Success<T> (T value)
        {
            return new UnitResultV2<T>(true, value);
        }

        public static UnitResultV2<T> Failure<T>(ErrorInfo error)
        {
            return new UnitResultV2<T>(false, error: error);
        }



    }



    public class UnitResultV2<T> : UnitResultV2
    {
        public T Result { get;}

        internal UnitResultV2(bool isSuccess,  T result = default(T) , ErrorInfo error = null)
            : base(isSuccess,error)
        {
            Result = result;
        }
    }
}
