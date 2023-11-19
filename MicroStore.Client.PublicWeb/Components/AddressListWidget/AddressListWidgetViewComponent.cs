using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace MicroStore.Client.PublicWeb.Components.AddressListWidget
{
    [Widget(
        AutoInitialize = true,
        RefreshUrl = "/Widget/AddressListWidget",
        ScriptFiles = new string[] { "/Pages/Shared/Components/AddressListWidget/address-list-widget.js" }
        )]
    [Authorize]
    public class AddressListWidgetViewComponent : AbpViewComponent
    {
        private readonly UserAddressService _userAddressService;

        public AddressListWidgetViewComponent(UserAddressService userAddressService)
        {
            _userAddressService = userAddressService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAddresses = await _userAddressService.ListAsync();

            var model = new AddressListWidgetModel
            {
                Addresses = userAddresses
            };

            return View(model);
        }
    }

    public class AddressListWidgetModel
    {
       public List<Address> Addresses { get; set; }
    }
}
