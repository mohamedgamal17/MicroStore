using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Geographic.Application.StateProvinces
{
    public class StateProvinceApplicationService : GeographicApplicationService, IStateProvinceApplicationService
    {
        private readonly IRepository<Country> _countryRepository;

        public StateProvinceApplicationService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<ResultV2<StateProvinceDto>> CreateAsync(string countryId, StateProvinceModel model, CancellationToken cancellationToken = default)
        {

            var country = await _countryRepository.SingleOrDefaultAsync(x => x.Id == countryId,cancellationToken);

            if(country == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException(typeof(Country),countryId));
            }

            var validationResult = ValidateStateProvince(country, model);

            if (validationResult.IsFailure)
            {
                return new ResultV2<StateProvinceDto>(validationResult.Exception);
            }


            var state = ObjectMapper.Map<StateProvinceModel, StateProvince>(model);


            country.StateProvinces.Add(state);

            await _countryRepository.UpdateAsync(country, cancellationToken : cancellationToken);

            return ObjectMapper.Map<StateProvince, StateProvinceDto>(state);
        }


        public async Task<ResultV2<StateProvinceDto>> UpdateAsync(string countryId, string stateProvinceId, StateProvinceModel model, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);


            if (country == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException(typeof(Country), countryId));
            }

            var validationResult = ValidateStateProvince(country, model,stateProvinceId);

            if (validationResult.IsFailure)
            {
                return new ResultV2<StateProvinceDto>(validationResult.Exception);
            }

            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if(state == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException(typeof(StateProvince), stateProvinceId));

            }

            state = ObjectMapper.Map(model, state);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return ObjectMapper.Map<StateProvince, StateProvinceDto>(state);


        }

        public async Task<ResultV2<Unit>> DeleteAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(Country), countryId));
            }


            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if (state == null)
            {
                return new ResultV2<Unit>(new EntityNotFoundException(typeof(StateProvince), stateProvinceId));

            }

            country.StateProvinces.Remove(state);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return Unit.Value;
        }

        public async Task<ResultV2<StateProvinceDto>> GetAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);


            if (country == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException(typeof(Country), countryId));
            }


            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if (state == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException(typeof(StateProvince), stateProvinceId));

            }


            return ObjectMapper.Map<StateProvince, StateProvinceDto>(state);
        }

        public async Task<ResultV2<StateProvinceDto>> GetByCodeAsync(string countryCode, string stateCode, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.TwoLetterIsoCode == countryCode || x.ThreeLetterIsoCode == countryCode, cancellationToken);

            if (country == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException($"Country with code : {countryCode} is not exist"));
            }

            var state = country.StateProvinces.SingleOrDefault(x => x.Abbreviation == stateCode);

            if (state == null)
            {
                return new ResultV2<StateProvinceDto>(new EntityNotFoundException($"State province with abbrevation : {stateCode} is not exist in country : {country.Name}"));
            }

            return ObjectMapper.Map<StateProvince, StateProvinceDto>(state);
        }


        public async Task<ResultV2<List<StateProvinceDto>>> ListAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return new ResultV2<List<StateProvinceDto>>(new EntityNotFoundException(typeof(Country), countryId));
            }


            return ObjectMapper.Map<List<StateProvince>, List<StateProvinceDto>>(country.StateProvinces);

        }



        private  ResultV2<Unit> ValidateStateProvince(Country country ,StateProvinceModel model, string? stateProvinceId = null
            )
        {
            var states = country.StateProvinces;

            if(stateProvinceId != null)
            {
                states = states.Where(x => x.Id != stateProvinceId).ToList();
            }


            if(states.Any(x=> x.Name == model.Name))
            {
                return new ResultV2<Unit>(new BusinessException($"State Province with name : {model.Name} is already in exist in country : {country.Name}"));

            }


            if(states.Any(x=> x.Abbreviation == model.Abbreviation))
            {
                return new ResultV2<Unit>(new BusinessException($"State Province with abbreviation : {model.Abbreviation} is already in exist in country : {country.Name}"));
            }

            return Unit.Value;
        }
    }
}
