﻿using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderListQuery : PagingQueryParams , IQuery
    {
    }
}