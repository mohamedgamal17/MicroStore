using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Manufacturers;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class ManufacturerController : AdministrationController
    {
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerController(ManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public IActionResult Index()
        {
            return View(new ManufacturerSearchModel());
        }

        
        [HttpPost]
        public async Task<IActionResult> Index(ManufacturerSearchModel model)
        {
            var requestOptions = new ManufacturerListRequestOptions()
            {
                Name = model.Name
            };

            var result = await _manufacturerService.ListAsync(requestOptions);

            var responseModel = new ManufacturerListModel
            {
                Data = ObjectMapper.Map<List<Manufacturer>, List<ManufacturerVM>>(result),
                Draw = model.Draw
            };

            return Json(responseModel);
        }

        [RuleSetForClientSideMessages("*")]
        public IActionResult Create()
        {
            return View(new ManufacturerModel());
        }


        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Create(ManufacturerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestOptions = ObjectMapper.Map<ManufacturerModel, ManufacturerRequestOptions>(model);

            var result =  await _manufacturerService.CreateAsync(requestOptions);

            return RedirectToAction("Index");
        }


        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(string id)
        {
            var manufacturer = await _manufacturerService.GetAsync(id);

            var model = ObjectMapper.Map<Manufacturer, ManufacturerModel>(manufacturer);

            ViewBag.Manufacturer = ObjectMapper.Map<Manufacturer, ManufacturerVM>(manufacturer);

            return View(model);
        }

        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(string id , ManufacturerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var requestOptions = ObjectMapper.Map<ManufacturerModel, ManufacturerRequestOptions>(model);

            var result = await _manufacturerService.UpdateAsync(id,requestOptions);

            return RedirectToAction("Edit", new { id = id });
        }
    }
}
