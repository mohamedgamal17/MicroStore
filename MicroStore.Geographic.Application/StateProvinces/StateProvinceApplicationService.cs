using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;
using Volo.Abp.Application.Services;
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

        public async Task<UnitResult<StateProvinceDto>> CreateAsync(string countryId, StateProvinceModel model, CancellationToken cancellationToken = default)
        {
      //      var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await _countryRepository.SingleOrDefaultAsync(x => x.Id == countryId,cancellationToken);

            if(country == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }

            var validationResult = ValidateStateProvince(country, model);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<StateProvinceDto>(validationResult.Error);
            }


            var state = ObjectMapper.Map<StateProvinceModel, StateProvince>(model);


            country.StateProvinces.Add(state);

            await _countryRepository.UpdateAsync(country, cancellationToken : cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<StateProvince, StateProvinceDto>(state));
        }


        public async Task<UnitResult<StateProvinceDto>> UpdateAsync(string countryId, string stateProvinceId, StateProvinceModel model, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }

            var validationResult = ValidateStateProvince(country, model,stateProvinceId);

            if (validationResult.IsFailure)
            {
                return UnitResult.Failure<StateProvinceDto>(validationResult.Error);
            }

            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if(state == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"State province with id : {stateProvinceId} is not exist in country : {country.Name}"));

            }

            state = ObjectMapper.Map(model, state);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return UnitResult.Success(ObjectMapper.Map<StateProvince, StateProvinceDto>(state));


        }

        public async Task<UnitResult> DeleteAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }


            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if (state == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"State province with id : {stateProvinceId} is not exist in country : {country.Name}"));

            }

            country.StateProvinces.Remove(state);

            await _countryRepository.UpdateAsync(country, cancellationToken: cancellationToken);

            return UnitResult.Success();
        }

        public async Task<UnitResult<StateProvinceDto>> GetAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }


            var state = country.StateProvinces.SingleOrDefault(x => x.Id == stateProvinceId);

            if (state == null)
            {
                return UnitResult.Failure<StateProvinceDto>(ErrorInfo.NotFound($"State province with id : {stateProvinceId} is not exist in country : {country.Name}"));

            }

            return UnitResult.Success(ObjectMapper.Map<StateProvince, StateProvinceDto>(state));
        }

        public async Task<UnitResult<List<StateProvinceDto>>> ListAsync(string countryId, CancellationToken cancellationToken = default)
        {
            var query = await _countryRepository.WithDetailsAsync(x => x.StateProvinces);

            var country = await query.SingleOrDefaultAsync(x => x.Id == countryId, cancellationToken);

            if (country == null)
            {
                return UnitResult.Failure<List<StateProvinceDto>>(ErrorInfo.NotFound($"Country with id : {countryId} is not exist"));
            }

            return UnitResult.Success(ObjectMapper.Map<List<StateProvince>, List<StateProvinceDto>>(country.StateProvinces));

        }



        private UnitResult ValidateStateProvince(Country country ,StateProvinceModel model, string? stateProvinceId = null
            )
        {
            var states = country.StateProvinces;

            if(stateProvinceId != null)
            {
                states = states.Where(x => x.Id != stateProvinceId).ToList();
            }


            if(states.Any(x=> x.Name == model.Name))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"State Province with name : {model.Name} is already in exist in country : {country.Name}"));
            }


            if(states.Any(x=> x.Abbreviation == model.Abbreviation))
            {
                return UnitResult.Failure(ErrorInfo.BusinessLogic($"State Province with abbreviation : {model.Abbreviation} is already in exist in country : {country.Name}"));
            }

            return UnitResult.Success();
        }
    }

    public interface IStateProvinceApplicationService : IApplicationService
    {
        Task<UnitResult<StateProvinceDto>> CreateAsync(string countryId, StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<StateProvinceDto>> UpdateAsync(string countryId,string stateProvinceId ,StateProvinceModel model, CancellationToken cancellationToken = default);

        Task<UnitResult> DeleteAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);

        Task<UnitResult<List<StateProvinceDto>>> ListAsync(string countryId,CancellationToken cancellationToken = default);

        Task<UnitResult<StateProvinceDto>> GetAsync(string countryId, string stateProvinceId, CancellationToken cancellationToken = default);
    }
}
