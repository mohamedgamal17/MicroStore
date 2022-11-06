using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Products.Queries
{
    internal class GetProductQueryHandler : QueryHandler<GetProductQuery, ProductDto>
    {
        private readonly ICatalogDbContext _catalogDbContext;



        public GetProductQueryHandler(ICatalogDbContext catalogDbContext)
        {
            _catalogDbContext = catalogDbContext;

        }

        public override async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {

            Product? product = await _catalogDbContext.Products
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
            {
                throw new EntityNotFoundException(typeof(Product), request.Id);
            }

            return ObjectMapper.Map<Product, ProductDto>(product);
        }


    }
}
