namespace MicroStore.BuildingBlocks.Results
{
    public interface IResult
    {
        bool IsSuccess { get; }

        bool IsFailure { get; }
        object Error { get; }

    }

    public interface IResult<TValue> : IResult
    {
        TValue Value { get; }
    }
}