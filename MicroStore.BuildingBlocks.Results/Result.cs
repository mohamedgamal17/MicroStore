namespace MicroStore.BuildingBlocks.Results
{
    public class Result 
 
    {
        private bool _isSucess;

        private string _error;

        private readonly string _errorType;   
        public bool IsSuccess => _isSucess;
        public bool IsFailure => !_isSucess;
        public string Error => _error;
        public string ErrorType => _errorType;
      
        internal Result(bool isSucess, string errorType,  string error)
        {
            _isSucess = isSucess;
            _errorType = errorType;
            _error = error;

        }


        public static Result Success()
        {
            return new Result(true, string.Empty, string.Empty);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(true, value, string.Empty,string.Empty);    
        }

        public static Result Failure(string errorType , string error)
        {
            return new Result(false, errorType, error);
        }

        public static Result<T> Failure<T>(string errorType, string error)
        {
            return new Result<T>(false,default(T), errorType, error);
        }
    }


    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException(Error);
                }

                return _value;
            }
        }

        internal Result(bool isSucess, T value , string errorType ,string error) 
            : base(isSucess,errorType ,error)
        {
            _value = value;
        }

    }
}
