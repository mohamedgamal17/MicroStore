using MicroStore.BuildingBlocks.Results;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Geographic.Application.Countries
{
    public interface ICountryApplicationService : IApplicationService
    {
        Task<UnitResult<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<CountryDto>> UpdateAsync(string countryId, CountryModel country , CancellationToken cancellationToken = default);

        Task<UnitResult> DeleteAsync(string countryId, CancellationToken cancellationToken = default);

        Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default);
        
        Task<UnitResult<CountryDto>> GetAsync(string countryId, CancellationToken cancellationToken = default);

        Task<UnitResult<CountryDto>> GetByCodeAsync(string countryCode,  CancellationToken cancellationToken = default);
    }
}
