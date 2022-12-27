namespace MicroStore.BuildingBlocks.Results
{
    public class UnitResult : Result
    {
        private readonly string _errorType;

        public string ErrorType => _errorType;

        internal UnitResult(bool isSucess, string errorType,  string error)
            : base(isSucess, error)
        {
            _errorType= errorType;

        }


        public static UnitResult Success()
        {
            return new UnitResult(true, string.Empty, string.Empty);
        }


        public static UnitResult<T> Success<T>(T value)
        {
            return new UnitResult<T>(true, value, string.Empty,string.Empty);    
        }

        public static UnitResult Failure(string errorType , string error)
        {
            return new UnitResult(false, errorType, error);
        }

        public static UnitResult<T> Failure<T>(string errorType, string error)
        {
            return new UnitResult<T>(false,default(T), errorType, error);
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
                    throw new InvalidOperationException("result is already failured");
                }

                return _value;
            }
        }

        internal UnitResult(bool isSucess, T value , string errorType ,string error) : base(isSucess,errorType ,error)
        {
            _value = value;
        }

    }
}
