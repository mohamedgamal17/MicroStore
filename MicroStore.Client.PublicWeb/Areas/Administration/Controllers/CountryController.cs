using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    public class CountryController : AdministrationController
    {
        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;
        public CountryController(CountryService countryService, StateProvinceService stateProvinceService)
        {
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
        }
        public IActionResult Index()
        {
            return View(new CountryListModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CountryListModel model)
        {
            var data = await _countryService.ListAsync();

            model.Data = ObjectMapper.Map<List<Country>, List<CountryVM>>(data);

            return Json(model);
        }
        public IActionResult Create()
        {
            return View(new CountryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestOptions = ObjectMapper.Map<CountryModel, CountryRequestOptions>(model);

            var result = await _countryService.CreateAsync(requestOptions);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var country = await _countryService.GetAsync(id);

            var model = ObjectMapper.Map<Country, CountryModel>(country);

            ViewBag.Country = ObjectMapper.Map<Country, CountryVM>(country);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CountryModel model)
        {
            if (!ModelState.IsValid)
            {
                var country = await _countryService.GetAsync(model.Id);

                ViewBag.Country = ObjectMapper.Map<Country, CountryVM>(country);

                return View(model);
            }

            var requestOptions = ObjectMapper.Map<CountryModel, CountryRequestOptions>(model);

            await _countryService.UpdateAsync(model.Id, requestOptions);

            return RedirectToAction("Edit", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ListStateProvinces(string id,StateProvinceListModel model)
        {
            var states =  await _stateProvinceService.ListAsync(id);

            model.Data = ObjectMapper.Map<List<StateProvince>, List<StateProvinceVM>>(states);

            return Json(model);
        }


        public async Task<IActionResult> GetStateProvince(string id , string stateProvinceId)
        {
            var state = await _stateProvinceService.GetAsync(id, stateProvinceId);

            var result = ObjectMapper.Map<StateProvince, StateProvinceVM>(state);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStateProvince(StateProvinceModel model) 
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState);
            }


            var requestOptions = ObjectMapper.Map<StateProvinceModel, StateProvinceRequestOptions>(model);

            var result = await _stateProvinceService.CreateAsync(model.CountryId, requestOptions);

            return Json(ObjectMapper.Map<StateProvince, StateProvinceVM>(result));
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStateProvince(StateProvinceModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState);
            }

            var requestOptions = ObjectMapper.Map<StateProvinceModel, StateProvinceRequestOptions>(model);

            var result = await _stateProvinceService.UpdateAsync(model.CountryId,model.Id, requestOptions);

            return Json(ObjectMapper.Map<StateProvince, StateProvinceVM>(result));
        }
    }
}
