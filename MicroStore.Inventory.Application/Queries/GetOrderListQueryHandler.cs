﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Inventory.Application.Queries
{
    public class GetOrderListQueryHandler : QueryHandler<GetOrderListQuery>
    {
        private readonly IInventoyDbContext _inventoryDbContext;

        public GetOrderListQueryHandler(IInventoyDbContext inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public override async Task<ResponseResult> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _inventoryDbContext.Orders
                .AsNoTracking()
                .ProjectTo<OrderDto>(MapperAccessor.Mapper.ConfigurationProvider);

            var result = await query.PageResult(request.PageNumber, request.PageSize, cancellationToken);

            return Success(HttpStatusCode.OK, result);
        }
    }
}
