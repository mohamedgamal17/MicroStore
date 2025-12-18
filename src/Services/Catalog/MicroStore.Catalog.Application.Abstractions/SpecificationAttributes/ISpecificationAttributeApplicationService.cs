using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes
{
    public interface ISpecificationAttributeApplicationService
    {
        Task<Result<SpecificationAttributeDto>> CreateAsync(SpecificationAttributeModel model, CancellationToken cancellationToken = default);
        Task<Result<SpecificationAttributeDto>> UpdateAsync(string attributeId, SpecificationAttributeModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAsync(string attributeId, CancellationToken cancellationToken = default);
        Task<Result<SpecificationAttributeDto>> CreateOptionAsync(string attributeId, SpecificationAttributeOptionModel model, CancellationToken cancellationToken = default);
        Task<Result<SpecificationAttributeDto>> UpdateOptionAsync(string attributeId, string optionId, SpecificationAttributeOptionModel model, CancellationToken cancellationToken = default);
        Task<Result<SpecificationAttributeDto>> RemoveOptionAsync(string attributeId, string optionId, CancellationToken cancellationToken = default);
        Task<Result<List<ElasticSpecificationAttribute>>> ListAsync(CancellationToken cancellationToken = default);
        Task<Result<ElasticSpecificationAttribute>> GetAsync(string attributeId, CancellationToken cancellationToken = default);
        Task<Result<List<ElasticSpecificationAttributeOption>>> ListOptionsAsync(string attributeId, CancellationToken cancellationToken = default);
        Task<Result<ElasticSpecificationAttributeOption>> GetOptionAsync(string attributeId, string optionId, CancellationToken cancellationToken = default);
    }
}
