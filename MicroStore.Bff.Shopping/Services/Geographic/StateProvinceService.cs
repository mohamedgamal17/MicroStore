using MicroStore.Bff.Shopping.Data.Geographic;
using MicroStore.Bff.Shopping.Grpc.Geographic;
using MicroStore.Bff.Shopping.Models.Geographic;

namespace MicroStore.Bff.Shopping.Services.Geographic
{
    public class StateProvinceService
    {
        private readonly Grpc.Geographic.StateProvinceService.StateProvinceServiceClient _stateProvinceClient;

        public StateProvinceService(Grpc.Geographic.StateProvinceService.StateProvinceServiceClient stateProvinceClient)
        {
            _stateProvinceClient = stateProvinceClient;
        }


        public async Task<List<StateProvince>> ListAsync(string countryId)
        {
            var request = new StateProvinceListRequest { CountryId = countryId };

            var result = await _stateProvinceClient.GetListAsync(request);

            return result.Items.Select(MapStateProvince).ToList();
        }

        public async Task<StateProvince> GetAsync(string countryId , string stateProvinceId)
        {
            var request = new GetStateProvinceByIdRequest
            {
                CountryId = countryId,
                StateProvinceId = stateProvinceId,
            };

            var result = await _stateProvinceClient.GetByIdAsync(request);

            return MapStateProvince(result);
        }

        public async Task<StateProvince> GetByCodeAsync(string countryCode  , string abbrevation)
        {
            var request = new GetStateProvinceByCodeRequest
            {
                CountryCode = countryCode,
                StateProvinceCode = abbrevation
            };

            var result = await _stateProvinceClient.GetByCodeAsync(request);

            return MapStateProvince(result);
        }

        public async Task<StateProvince> CreateAsync(string countryId,StateProvinceModel model)
        {
            var reqeust = new CreateStateProvinceRequest
            {
                CountryId = countryId,
                Name = model.Name,
                Abbrevation = model.Abbreviation
            };

            var result = await _stateProvinceClient.CreateAsync(reqeust);

            return MapStateProvince(result);
        }

        public async Task<StateProvince> UpdateAsync(string countryId,string stateProvinceId ,StateProvinceModel model)
        {
            var request = new UpdateStateProvinceRequest
            {
                CountryId = countryId,
                StateProvinceId = stateProvinceId,
                Name = model.Name,
                Abbrevation = model.Abbreviation
            };

            var result = await _stateProvinceClient.UpdateAsync(request);

            return MapStateProvince(result);
        }

        private StateProvince MapStateProvince(StateProvinceResponse response)
        {
            return new StateProvince
            {
                Id = response.Id,
                Name = response.Name,
                Abbreviation = response.Abbrevation,
                CreatedAt = response.CreatedAt.ToDateTime(),
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };
        }
    }
}
