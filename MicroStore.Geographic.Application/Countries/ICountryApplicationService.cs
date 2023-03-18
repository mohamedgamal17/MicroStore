using MicroStore.BuildingBlocks.Results;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Geographic.Application.Countries
{
    public interface ICountryApplicationService : IApplicationService
    {
        Task<ResultV2<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<CountryDto>> UpdateAsync(string countryId, CountryModel country , CancellationToken cancellationToken = default);

        Task<ResultV2<Unit>> DeleteAsync(string countryId, CancellationToken cancellationToken = default);

        Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default);
        
        Task<ResultV2<CountryDto>> GetAsync(string countryId, CancellationToken cancellationToken = default);

        Task<ResultV2<CountryDto>> GetByCodeAsync(string countryCode,  CancellationToken cancellationToken = default);
    }
}
