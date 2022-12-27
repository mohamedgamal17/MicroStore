using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Products.Queries
{
    internal class GetProductQueryHandler : QueryHandler<GetProductQuery>
    {
        private readonly ICatalogDbContext _catalogDbContext;



        public GetProductQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;

        }

        public override async Task<ResponseResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {

            Product? product = await _catalogDbContext.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Product entity with id : {request.Id} is not found"
                });
            }

            return ResponseResult.Success((int) HttpStatusCode.OK , ObjectMapper.Map<Product, ProductDto>(product)) ;
        }


    }
}
