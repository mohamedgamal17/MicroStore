using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Geographic.Application.Countries
{
    public class CountryApplicationService : GeographicApplicationService, ICountryApplicationService
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryApplicationService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<UnitResult<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateCountry(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<CountryDto>(validationResult.Error);
            }

            var country =  ObjectMapper.Map<CountryModel, Country>(model);

            await _countryRepository.InsertAsync(country , cancellationToken : cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Country, CountryDto>(country));
        }
        public async Task<UnitResult<CountryDto>> UpdateAsync(string countryId, CountryModel model, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<CountryDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }

            var validationResult = await ValidateCountry(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<CountryDto>(validationResult.Error);
            }

            country =  ObjectMapper.Map(model, country);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Country, CountryDto>(country));
        }

        public async Task<UnitResult> DeleteAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country  = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<CountryDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }

            await _countryRepository.DeleteAsync(country, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<Country, CountryDto>(country));

        }

        public async Task<UnitResult<CountryDto>> GetAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if(country == null)
            {
                return UnitResult.Failure<CountryDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));

            }

            return UnitResult.Success(ObjectMapper.Map<Country, CountryDto>(country));
        }

        public async Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = await _countryRepository.ToListAsync(cancellationToken);

            return ObjectMapper.Map<List<Country>, List<CountryListDto>>(result);

        }

        private async Task<UnitResult> ValidateCountry(CountryModel model , string? countryId = null ,CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.GetQueryableAsync();

            if(countryId != null)
            {
                query = query.Where(x => x.Id != countryId);
            }

            if(await _countryRepository.AnyAsync(x=> x.Name == model.Name))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Country name : {model.Name} is already exist"));
            }

            if(await _countryRepository.AnyAsync(x=> x.TwoLetterIsoCode == model.TwoLetterIsoCode))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Country two letter iso code : {model.TwoLetterIsoCode} is already exist"));
            }

            if (await _countryRepository.AnyAsync(x => x.ThreeLetterIsoCode == model.ThreeLetterIsoCode))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"Country three letter iso code : {model.ThreeLetterIsoCode} is already exist"));
            }

            return UnitResult.Success();
        }
    }
}
