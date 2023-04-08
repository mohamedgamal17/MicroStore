﻿using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{

    public class CategoryController : AdministrationController
    {

        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            NotificationManager.Error("Test", "Kosm Modern Acedemy");
            NotificationManager.Info("Test", "Kosm Modern Acedemy");

            NotificationManager.Warning("Test", "Kosm Modern Acedemy");

            return View(new CategoryListModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(CategoryListModel model)
        {
            var data = await _categoryService.ListAsync(new SortingRequestOptions ());

            model.Data = ObjectMapper.Map<List<Category>, List<CategoryVM>>(data);


            return Json(model);
        }

        public IActionResult Create()
        {
            return View(new CategoryModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryRequestOptions = ObjectMapper.Map<CategoryModel, CategoryRequestOptions>(model);

            try
            {
                await _categoryService.CreateAsync(categoryRequestOptions);

                return RedirectToAction("Index");
            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return View(model);
            }
        }


     
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _categoryService.GetAsync(id);

            var model = ObjectMapper.Map<Category, CategoryModel>(category);

            return View(model);
        }



        [HttpPost]
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

                return RedirectToAction("Index");

            }catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return View(model);
            }
        }


       

    }
}
