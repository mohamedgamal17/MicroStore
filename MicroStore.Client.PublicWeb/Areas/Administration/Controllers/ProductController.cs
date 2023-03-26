using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    public class ProductController : AdministrationController
    {

        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;

        private readonly ILogger<ProductController> _logger;
        public ProductController(ProductService productService, CategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
                 
            return View(new ProductListModel());
            
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductListModel model)
        {
            var data = await _productService.ListAsync(new PagingAndSortingRequestOptions { Skip = model.Skip, Lenght = model.Length });

            model.Data = ObjectMapper.Map<List<Product>, List<ProductVM>>(data.Items);

            model.RecordsTotal = data.TotalCount;

            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories =  await BuildCategoriesSelecetListItems();

            return View(new ProductModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }

            try
            {
                var requestOptions = ObjectMapper.Map<ProductModel, ProductRequestOptions>(model);

                await _productService.CreateAsync(requestOptions);

                return RedirectToPage("Index");

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest) 
            {
                ex.Erorr.MapToModelState(ModelState);

                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productService.GetAsync(id);

            var model = ObjectMapper.Map<Product, ProductModel>(product);

            ViewBag.Categories = await BuildCategoriesSelecetListItems();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await BuildCategoriesSelecetListItems();
                _logger.LogInformation("Model : {Model}", model);
                _logger.LogInformation("Long description {LongDescription}", model.LongDescription);

                return View(model);
            }


            try
            {
                var requestOptions = ObjectMapper.Map<ProductModel, ProductRequestOptions>(model);

                await _productService.UpdateAsync(model.Id,requestOptions);

                return RedirectToAction("Index");

            }catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }
        }


        private async Task<List<SelectListItem>> BuildCategoriesSelecetListItems(string[]? categoriesIds = null)
        {
            var categorySelectItems =  new List<SelectListItem>();

            var categories = await _categoryService.ListAsync(new SortingRequestOptions { Desc = true });

         
            if (categories != null)
            {
                categorySelectItems = categories.Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = categoriesIds?.Contains(x.Id) ?? false }).ToList();

            }

            return categorySelectItems;
        }
            


        private List<ProductVM> LoadTestData()
        {
            return new List<ProductVM>
            {
                new ProductVM
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    Price = 50,
                },
                 new ProductVM
                {
                     Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    Price = 50,
                },
                  new ProductVM
                {
                      Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    Price = 50,
                },
                 new ProductVM
                {
                     Id = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString(),
                    Sku = Guid.NewGuid().ToString(),
                    Price = 50,
                },
            };

        }
    }
}
