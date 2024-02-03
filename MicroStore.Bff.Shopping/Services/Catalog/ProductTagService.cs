using Google.Protobuf;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Grpc.Catalog;
using MicroStore.Bff.Shopping.Models.Catalog.ProductTags;
namespace MicroStore.Bff.Shopping.Services.Catalog
{
    public class ProductTagService
    {
        private readonly Grpc.Catalog.TagService.TagServiceClient _tagServiceClient;

        public ProductTagService(TagService.TagServiceClient tagServiceClient)
        {
            _tagServiceClient = tagServiceClient;
        }

        public async Task<List<ProductTag>> ListAsync(CancellationToken cancellationToken = default)
        {
            var request = new TagListRequest();

            var response = await _tagServiceClient.GetListAsync(request);

            var result = response.Data.Select(PrepareProductTag).ToList();

            return result;
        }

        public async Task<ProductTag> GetAsync(string tagId, CancellationToken cancellationToken = default)
        {
            var request = new GetTagByIdRequest { Id = tagId };

            var response = await _tagServiceClient.GetbyIdAsync(request);

            return PrepareProductTag(response);
        }
        public async Task<ProductTag> CreateAsync(ProductTagModel model, CancellationToken cancellationToken = default)
        {
            var request = new CreateTagRequest
            {
                Name = model.Name,
                Description = model.Description
            };

            var response = await _tagServiceClient.CreateAsync(request);

            return PrepareProductTag(response);
        }

        public async Task<ProductTag> UpdateAsync(string tagId , ProductTagModel model , CancellationToken cancellationToken = default)
        {
            var request = new UpdateTagRequest
            {
                Id = tagId,
                Name = model.Name,
                Description = model.Description
            };

            var response = await _tagServiceClient.UpdateAsync(request);

            return PrepareProductTag(response);
        }

        public ProductTag PrepareProductTag(TagResponse response)
        {
            return new ProductTag
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };
        }
    }
}
