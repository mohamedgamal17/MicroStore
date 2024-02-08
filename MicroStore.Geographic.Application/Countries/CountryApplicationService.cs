using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateCountry(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<CountryDto>(validationResult.Exception);
            }

            var country =  ObjectMapper.Map<CountryModel, Country>(model);

            await _countryRepository.InsertAsync(country , cancellationToken : cancellationToken);

            return ObjectMapper.Map<Country, CountryDto>(country);
        }
        public async Task<Result<CountryDto>> UpdateAsync(string countryId, CountryModel model, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return  new Result<CountryDto>(new EntityNotFoundException(typeof(CountryDto), countryId)) ;
            }

            var validationResult = await ValidateCountry(model, countryId,cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new Result<CountryDto>(validationResult.Exception);
            }

            country =  ObjectMapper.Map(model, country);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        public async Task<Result<Unit>> DeleteAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country  = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new Result<Unit>(new EntityNotFoundException(typeof(CountryDto), countryId));
            }

            await _countryRepository.DeleteAsync(country, cancellationToken: cancellationToken);

            return Unit.Value;

        }

        public async Task<Result<CountryDto>> GetAsync(string countryId,  CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new Result<CountryDto>(new EntityNotFoundException(typeof(CountryDto), countryId));
            }
            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        public async Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = await _countryRepository.ToListAsync(cancellationToken);

            return ObjectMapper.Map<List<Country>, List<CountryListDto>>(result);

        }
        
        public async Task<Result<CountryDto>> GetByCodeAsync(string countryCode  , CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x=> x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.TwoLetterIsoCode  == countryCode || x.ThreeLetterIsoCode == countryCode, cancellationToken);
        
            if(country == null)
            {
                return new Result<CountryDto>(new EntityNotFoundException($"Country with code : {countryCode} is not exist"));
            }

            return ObjectMapper.Map<Country, CountryDto>(country);
        }
        private async Task<Result<Unit>> ValidateCountry(CountryModel model , string? countryId = null ,CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.GetQueryableAsync();

            if(countryId != null)
            {
                query = query.Where(x => x.Id != countryId);
            }

            if(await query.AnyAsync(x=> x.Name == model.Name))
            {
                return new Result<Unit>(new UserFriendlyException( $"Country name : {model.Name} is already exist"));
            }

            if(await query.AnyAsync(x=> x.TwoLetterIsoCode == model.TwoLetterIsoCode))
            {
                return new Result<Unit>(new UserFriendlyException($"Country two letter iso code : {model.TwoLetterIsoCode} is already exist"));
            }

            if (await query.AnyAsync(x => x.ThreeLetterIsoCode == model.ThreeLetterIsoCode))
            {
                return new Result<Unit>(new UserFriendlyException(message: $"Country three letter iso code : {model.ThreeLetterIsoCode} is already exist"));
            }

            return Unit.Value;
        }

        public async Task<List<CountryDto>> ListByCodesAsync(List<string> codes, CancellationToken cancellationToken = default)
        {
            if(codes != null && codes.Count > 0)
            {
                var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

                var result = await query.Where(x => codes.Contains(x.TwoLetterIsoCode) || codes.Contains(x.ThreeLetterIsoCode)).ToListAsync(cancellationToken);

                return ObjectMapper.Map<List<Country>, List<CountryDto>>(result);
            }

            return new List<CountryDto>();
        }
    }
}
