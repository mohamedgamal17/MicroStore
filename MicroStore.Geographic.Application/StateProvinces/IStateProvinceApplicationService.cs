using MicroStore.BuildingBlocks.Results;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Geographic.Application.StateProvinces
{
    public interface IStateProvinceApplicationService : IApplicationService
    {
        Task<ResultV2<StateProvinceDto>> CreateAsync(string countryId, StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<StateProvinceDto>> UpdateAsync(string countryId,string stateProvinceId ,StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<Unit>> DeleteAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);

        Task<ResultV2<List<StateProvinceDto>>> ListAsync(string countryId,CancellationToken cancellationToken = default);

        Task<ResultV2<StateProvinceDto>> GetAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);

        Task<ResultV2<StateProvinceDto>> GetByCodeAsync(string countryCode , string stateCode , CancellationToken cancellationToken = default);
    }
}
