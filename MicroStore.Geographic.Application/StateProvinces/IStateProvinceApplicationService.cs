using MicroStore.BuildingBlocks.Results;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Geographic.Application.StateProvinces
{
    public interface IStateProvinceApplicationService : IApplicationService
    {
        Task<UnitResult<StateProvinceDto>> CreateAsync(string countryId, StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<StateProvinceDto>> UpdateAsync(string countryId,string stateProvinceId ,StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<UnitResult> DeleteAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);

        Task<UnitResult<List<StateProvinceDto>>> ListAsync(string countryId,CancellationToken cancellationToken = default);

        Task<UnitResult<StateProvinceDto>> GetAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);

        Task<UnitResult<StateProvinceDto>> GetByCodeAsync(string countryCode , string stateCode , CancellationToken cancellationToken = default);
    }
}
