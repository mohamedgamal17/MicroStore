﻿using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
namespace MicroStore.Catalog.Application.Products.Queries
{
    internal class GetProductListQueryHandler : QueryHandler<GetProductListQuery>
    {

        private readonly ICatalogDbContext _catalogDbContext;

        public GetProductListQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;
        }

        public override async Task<ResponseResult> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var result = await _catalogDbContext.Products
                .ToListAsync(cancellationToken);

            return ResponseResult.Success((int) HttpStatusCode.OK , ObjectMapper.Map<List<Product>, List<ProductDto>>(result)) ;
        }
    }
}
