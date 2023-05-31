using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.CheckoutInformation
{
    [Widget(AutoInitialize = true,
        RefreshUrl = "Widget/CheckoutInformation",
        ScriptTypes = new Type[] {typeof(CheckoutInformationWidgetScriptContrinutor) })]
    public class CheckoutInformationWidgetViewComponent : AbpViewComponent
    {
        private readonly CountryService _countryService;

        public CheckoutInformationWidgetViewComponent(CountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var countries = await _countryService.ListAsync();

            if(countries.Count <= 0)
            {
                ViewBag.CheckoutInformationWidgetError = true;

                ViewBag.CheckoutInformationWidgetErrorDetails = "Please Contact Admin to update geogrphic service";

            }

            ViewBag.Countries = countries.Select(x => new SelectListItem
            {
                Text = x.Name,

                Value = x.TwoLetterIsoCode

            }).ToList();

            return View();
        }
    }
}
