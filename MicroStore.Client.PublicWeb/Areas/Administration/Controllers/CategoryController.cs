using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser , Roles = ApplicationSecurityRoles.Admin)]
    public class CategoryController : AdministrationController
    {

        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new CategorySearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CategorySearchModel model)
        {
            var requestOptions = new CategoryListRequestOptions
            {
                Name = model.Name
            };

            var data = await _categoryService.ListAsync(requestOptions);

            var responseModel = new CategoryListModel
            {
                Data = ObjectMapper.Map<List<Category>, List<CategoryVM>>(data),
                Draw = model.Draw
            };

            return Json(responseModel);
        }
        [RuleSetForClientSideMessages("*")]
        public IActionResult Create()
        {
            return View(new CategoryModel());
        }

        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryRequestOptions = ObjectMapper.Map<CategoryModel, CategoryRequestOptions>(model);

            try
            {
                var category = await _categoryService.CreateAsync(categoryRequestOptions);

                NotificationManager.Success("Category has been created successfully !");

                return RedirectToAction("Edit" , new { id= category.Id });
            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return View(model);
            }
        }


        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _categoryService.GetAsync(id);

            var model = ObjectMapper.Map<Category, CategoryModel>(category);

            return View(model);
        }



        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryRequestOptions = ObjectMapper.Map<CategoryModel, CategoryRequestOptions>(model);

            try
            {
                await _categoryService.UpdateAsync(model.Id, categoryRequestOptions);

                NotificationManager.Success("Category has been updated successfully !");

                return RedirectToAction("Edit", new {id = model.Id});

            }catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return View(model);
            }
        }


       

    }
}
