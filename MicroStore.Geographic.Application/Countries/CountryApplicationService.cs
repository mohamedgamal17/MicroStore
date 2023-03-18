using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
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

        public async Task<ResultV2<CountryDto>> CreateAsync(CountryModel model, CancellationToken cancellationToken = default)
        {
            var validationResult = await ValidateCountry(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<CountryDto>(validationResult.Exception);
            }

            var country =  ObjectMapper.Map<CountryModel, Country>(model);

            await _countryRepository.InsertAsync(country , cancellationToken : cancellationToken);

            return ObjectMapper.Map<Country, CountryDto>(country);
        }
        public async Task<ResultV2<CountryDto>> UpdateAsync(string countryId, CountryModel model, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return  new ResultV2<CountryDto>(new EntityNotFoundException(typeof(CountryDto), countryId)) ;
            }

            var validationResult = await ValidateCountry(model, cancellationToken: cancellationToken);

            if (validationResult.IsFailure)
            {
                return new ResultV2<CountryDto>(validationResult.Exception);
            }

            country =  ObjectMapper.Map(model, country);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        public async Task<ResultV2<Unit>> DeleteAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country  = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(CountryDto), countryId));
            }

            await _countryRepository.DeleteAsync(country, cancellationToken: cancellationToken);

            return Unit.Value;

        }

        public async Task<ResultV2<CountryDto>> GetAsync(string countryId,  CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new ResultV2<CountryDto>(new EntityNotFoundException(typeof(CountryDto), countryId));
            }
            return ObjectMapper.Map<Country, CountryDto>(country);
        }

        public async Task<List<CountryListDto>> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = await _countryRepository.ToListAsync(cancellationToken);

            return ObjectMapper.Map<List<Country>, List<CountryListDto>>(result);

        }
        
        public async Task<ResultV2<CountryDto>> GetByCodeAsync(string countryCode  , CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x=> x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.TwoLetterIsoCode  == countryCode || x.ThreeLetterIsoCode == countryCode, cancellationToken);
        
            if(country == null)
            {
                return new ResultV2<CountryDto>(new EntityNotFoundException($"Country with code : {countryCode} is not exist"));
            }

            return ObjectMapper.Map<Country, CountryDto>(country);
        }
        private async Task<ResultV2<Unit>> ValidateCountry(CountryModel model , string? countryId = null ,CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.GetQueryableAsync();

            if(countryId != null)
            {
                query = query.Where(x => x.Id != countryId);
            }

            if(await _countryRepository.AnyAsync(x=> x.Name == model.Name))
            {
                return new ResultV2<Unit>(new BusinessException($"Country name : {model.Name} is already exist"));
            }

            if(await _countryRepository.AnyAsync(x=> x.TwoLetterIsoCode == model.TwoLetterIsoCode))
            {
                return new ResultV2<Unit>(new BusinessException($"Country two letter iso code : {model.TwoLetterIsoCode} is already exist"));
            }

            if (await _countryRepository.AnyAsync(x => x.ThreeLetterIsoCode == model.ThreeLetterIsoCode))
            {
                return new ResultV2<Unit>(new BusinessException($"Country three letter iso code : {model.ThreeLetterIsoCode} is already exist"));
            }

            return Unit.Value;
        }
    }
}
