using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class CountryController : AdministrationController
    {
        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;

        private readonly CountryAnalysisService _countryAnalysisService;
        public CountryController(CountryService countryService, StateProvinceService stateProvinceService, CountryAnalysisService countryAnalysisService)
        {
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _countryAnalysisService = countryAnalysisService;
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

        [RuleSetForClientSideMessages("*")]
        public IActionResult Create()
        {
            return View(new CountryModel());
        }

        [HttpPost]
        [RuleSetForClientSideMessages("*")]
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

        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(string id)
        {
            var country = await _countryService.GetAsync(id);

            var model = ObjectMapper.Map<Country, CountryModel>(country);

            ViewBag.Country = ObjectMapper.Map<Country, CountryVM>(country);

            return View(model);
        }


        [HttpPost]
        [RuleSetForClientSideMessages("*")]
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

        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> CreateOrEditStateModal(string countryId , string? stateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new StateProvinceModel
            {
                CountryId = countryId
            };

            if(stateId != null)
            {
                var state = await _stateProvinceService.GetAsync(countryId, stateId);
                model.Id = state.Id;
                model.Name = state.Name;
                model.Abbreviation = state.Abbreviation;
            }


            return PartialView("_CreateOrUpdate.State", model);
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
                return BadRequest(ModelState);
            }

            var requestOptions = ObjectMapper.Map<StateProvinceModel, StateProvinceRequestOptions>(model);

            var result = await _stateProvinceService.UpdateAsync(model.CountryId,model.Id, requestOptions);

           
            return Json(ObjectMapper.Map<StateProvince, StateProvinceVM>(result));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStateProvince([FromBody] RemoveStateProvinceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await  _stateProvinceService.DeleteAsync(model.CountryId, model.StateId);

            return NoContent();
        }

        public async Task<IActionResult> SalesReport(string code)
        {
            var response = await _countryService.GetByCodeAsync(code);

            ViewBag.Countries = ObjectMapper.Map<Country, CountryVM>(response);

            return View(new CountrySalesReportModel());
        }

        [HttpPost]
        public async Task<IActionResult> SalesReport(string code,CountrySalesReportModel model)
        {
            var requestOptions = new CountrySalesReportRequestOptions
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Period = model.GroupBy,
                Skip = model.Skip,
                Length = model.Length
            };

            var response = await _countryAnalysisService.GetCountrySalesReport(code, requestOptions);

            var responseModel = new CountrySalesReportModel
            {
                Data = ObjectMapper.Map<List<CountrySalesReport>, List<CountrySalesReportVM>>(response.Items),
                Draw =  model.Draw,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                GroupBy = model.GroupBy,
                Start = model.Start,
                Length = response.Lenght,
                RecordsTotal = response.TotalCount
            };

            return Json(responseModel);
        }

        public async Task<IActionResult> SalesForecasting(string code)
        {
            try
            {
                var response = await _countryService.GetByCodeAsync(code);

                ViewBag.Country = ObjectMapper.Map<Country, CountryVM>(response);

                var forcastedValues = await _countryAnalysisService.Forecast(code,new ForecastRequestOptions
                {
                    Horizon = 6,
                    ConfidenceLevel = 0.95f
                });

                var lastYearDate = DateTime.Now.AddMonths(-12);

                var startDate = new DateTime(lastYearDate.Year, lastYearDate.Month, 1);

                var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                var monthlySalesReport = await _countryAnalysisService.GetCountrySalesReport(code,new CountrySalesReportRequestOptions
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Period = ReportPeriod.Monthly,
                    Length = 12
                });

                var projection = monthlySalesReport.Items.OrderBy(x=> x.Date).Select(x => new CountrySalesChartDataModel
                {
                    TotalOrders = x.TotalOrders,
                    SumShippingTotalCost = x.TotalShippingPrice,
                    SumTaxTotalCost = x.TotalTaxPrice,
                    SumTotalCost = x.TotalPrice,
                    Date = x.Date.ToString("MM-dd-yyyy"),

                }).ToList();

                int monthOffset = 1;

                for (int i = 0; i < forcastedValues.ForecastedValues.Length; i++)
                {

                    projection.Add(new CountrySalesChartDataModel
                    {
                        SumTotalCost = forcastedValues.ForecastedValues[i],
                        Date = DateTime.Now.AddMonths(monthOffset).ToString("MM-dd-yyyy"),
                        IsForecasted = true
                    });

                    monthOffset++;
                }

                return View(projection);

            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                NotificationManager.Error(ex.Erorr.Detail);

                return RedirectToAction(nameof(CountrySalesReport), new { code = code });
            }
        }
    }
}
