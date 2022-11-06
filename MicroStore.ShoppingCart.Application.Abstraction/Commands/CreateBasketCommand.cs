using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.ShoppingCart.Application.Abstraction.Commands
{
    public class CreateBasketCommand : ICommand<BasketDto> { }
}
