using MicroStore.Bff.Shopping.Grpc.Geographic;
using MicroStore.Bff.Shopping.Data.Geographic;
using MicroStore.Bff.Shopping.Models.Geographic;
namespace MicroStore.Bff.Shopping.Services.Geographic
{
    public class CountryService 
    {
        private readonly Grpc.Geographic.CountryService.CountryServiceClient _countryServiceClient;

        public CountryService(Grpc.Geographic.CountryService.CountryServiceClient countryServiceClient)
        {
            _countryServiceClient = countryServiceClient;
        }

        public async Task<List<Country>> ListAsync()
        {
            var request = new CountryListRequest();

            var result = await _countryServiceClient.GetListAsync(request);

            return result.Items.Select(MapCountry).ToList();
        }

        public async Task<Country> GetById(string id)
        {
            var request = new GetCountryByIdRequest { Id = id };

            var result = await _countryServiceClient.GetByIdAsync(request);

            return MapCountry(result);
        }

        public async Task<Country> GetByCodeAsync(string code)
        {
            var request = new GetCountryByCodeRequest { Code = code };

            var result = await _countryServiceClient.GetByCodeAsync(request);

            return MapCountry(result);
        }

        public async Task<Country> CreateAsync(CountryModel model)
        {
            var request = new CreateCountryRequest
            {
                Name = model.Name,
                TwoLetterIsoCode = model.TwoLetterIsoCode,
                ThreeLetterIsoCode = model.ThreeLetterIsoCode,
                NumericIsoCode = model.NumericIsoCode
            };

            var result = await _countryServiceClient.CreateAsync(request);

            return MapCountry(result);
        }

        public async Task<Country> UpdateAsync(string id ,CountryModel model)
        {
            var request = new UpdateCountryRequest
            {
                Id = id,
                Name = model.Name,
                TwoLetterIsoCode = model.TwoLetterIsoCode,
                ThreeLetterIsoCode = model.ThreeLetterIsoCode,
                NumericIsoCode = model.NumericIsoCode
            };

            var result = await _countryServiceClient.UpdateAsync(request);

            return MapCountry(result);
        }

        private Country MapCountry(CountryResponse response)
        {
            var country = new Country
            {
                Id = response.Id,
                Name = response.Name,
                NumericIsoCode = response.NumericIsoCode,
                TwoLetterIsoCode = response.TwoLetterIsoCode,
                ThreeLetterIsoCode = response.ThreeLetterIsoCode,
                CreatedAt  =response.CreatedAt.ToDateTime() ,
                ModifiedAt = response.ModifiedAt?.ToDateTime()
            };

            if (response.States.Count > 0)
            {
                country.StateProvinces = response.States.Select(x => new StateProvince
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbrevation,
                    CreatedAt = response.CreatedAt.ToDateTime(),
                    ModifiedAt = response.ModifiedAt?.ToDateTime()
                }).ToList();
            }

            return country;
        }
    }
}
