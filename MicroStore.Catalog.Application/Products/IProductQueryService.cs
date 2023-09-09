using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Products;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Products
{
    public interface IProductQueryService
    {
        Task<Result<PagedResult<ElasticProduct>>> ListAsync(ProductListQueryModel queryParams,CancellationToken cancellationToken = default);
        Task<Result<List<ElasticProductImage>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default);
        Task<Result<ElasticProduct>> GetAsync(string id , CancellationToken cancellationToken = default );
        Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductSearchModel model , CancellationToken cancellationToken = default);
        Task<Result<List<ProductDto>>> SearchByImage(ProductSearchByImageModel model, CancellationToken cancellationToken = default);
    }


}
