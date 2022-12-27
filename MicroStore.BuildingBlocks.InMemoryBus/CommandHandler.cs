using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Uow;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class CommandHandler<TCommand> : RequestHandler<TCommand,ResponseResult> , IUnitOfWorkEnabled
        where TCommand : ICommand
    {

    }
}
