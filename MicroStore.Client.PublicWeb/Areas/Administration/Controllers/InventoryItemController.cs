using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Inventory;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class InventoryItemController : AdministrationController
    {
        private readonly InventoryItemService _inventoryItemService;

        public InventoryItemController(InventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(InventoryItemListModel model)
        {
            var requestOptions = new PagingAndSortingRequestOptions
            {
                Length = model.Length,
                Skip = model.Skip,
            };

            var data = await _inventoryItemService.ListAsync(requestOptions);

            model.Data = ObjectMapper.Map<List<InventoryItem>, List<InventoryItemVM>>(data.Items);

            model.RecordsTotal = data.TotalCount;

            return Json(model);
        }


        
        public async Task<IActionResult> Edit(string id)
        {
            var item  = await  _inventoryItemService.GetAsync(id);

            ViewBag.InventoryItem = ObjectMapper.Map<InventoryItem, InventoryItemVM>(item);

            var model = new InventoryItemModel
            {
                Id = item.Id,
                Stock = item.Stock
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( InventoryItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestOptions = new InventoryItemRequestOptions
            {
                Stock = model.Stock
            };

            await _inventoryItemService.UpdateAsync(model.Id, requestOptions);

            NotificationManager.Success("Product stock has been updated!");

            return RedirectToAction("Edit", new { model.Id});
        }
    }
}
