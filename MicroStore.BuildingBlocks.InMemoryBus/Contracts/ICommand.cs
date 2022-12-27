﻿

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {

    }

    public interface ICommand : ICommand<Unit>
    {

    }

    public interface ICommandV1 : IRequest<ResponseResult>
    {

    }
}
