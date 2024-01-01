namespace MicroStore.BuildingBlocks.Utils.Results
{
    public class StateMachineResult
    {
        public bool IsSuccess { get; set; }

        public string Error { get; set; } = string.Empty;

        public bool IsFailure => !IsSuccess;


        public static StateMachineResult Success()
        {
            return new StateMachineResult { IsSuccess = true };
        }

        public static StateMachineResult<TValue> Success<TValue>(TValue value)
        {
            return new StateMachineResult<TValue> { Value = value, IsSuccess = true };
        }

        public static StateMachineResult Failure(string error)
        {
            return new StateMachineResult { IsSuccess = false, Error = error };
        }

        public static StateMachineResult<TValue> Failure<TValue>(string error)
        {
            return new StateMachineResult<TValue> { IsSuccess = false, Error = error };
        }
    }


    public class StateMachineResult<TValue> : StateMachineResult
    {
        public TValue Value { get; set; }

    }
}
