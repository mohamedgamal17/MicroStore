using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class CommandHandler<TCommand, TResponse> : RequestHandler<TCommand, TResponse>
        , IUnitOfWorkEnabled where TCommand : ICommand<TResponse>
    {

    }


    public abstract class CommandHandler<TCommand> : CommandHandler<TCommand, Unit>
        where TCommand : ICommand
    {

    }
}
