using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Data;
using System.Net;
using Volo.Abp.BlobStoring;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]

    public class ProductController : AdministrationController
    {

        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;

        private readonly ILogger<ProductController> _logger;

        private readonly IBlobContainer<MultiMediaBlobContainer> _blobContainer;

        private readonly ManufacturerService _manufacturerService;
        public ProductController(ProductService productService, CategoryService categoryService, ILogger<ProductController> logger, IBlobContainer<MultiMediaBlobContainer> blobContainer, ManufacturerService manufacturerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
            _blobContainer = blobContainer;
            _manufacturerService = manufacturerService;
        }

        public async Task<IActionResult> Index()
        {

            ViewBag.Categories = await BuildCategoriesSelecetListItems(idDefaultValue : false);

            ViewBag.Manufacturers = await BuildManufacturersSelecetListItems(idDefaultValue: false);

            return View(new ProductSearchModel());

        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductSearchModel model)
        {
            var requestOptions = new ProductListRequestOptions
            {
                Name = model.Name,
                Category = model.Category,
                Manufacturer = model.Manufacturer,
                Tag = model.Tag,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Skip = model.Skip,
                Lenght = model.Length
            };

            var data = await _productService.ListAsync(requestOptions);

            var responseModel = new ProductListModel
            {
                Start = model.Start,
                Length = model.Length,
                Draw = model.Draw,
                RecordsTotal = model.RecordsTotal,
                Data = ObjectMapper.Map<List<Product>, List<ProductVM>>(data.Items)
            };

            return Json(responseModel);
        }

        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Create()
        {
            ControllerContext.SetRulesetForClientsideMessages("*");

            ViewBag.Categories =  await BuildCategoriesSelecetListItems();
            ViewBag.Categories = await BuildCategoriesSelecetListItems();

            ViewBag.Manufacturers = await BuildManufacturersSelecetListItems();

            return View(new ProductModel());
        }


        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (!ModelState.IsValid)
            {
        
                ViewBag.Categories = await BuildCategoriesSelecetListItems(model.CategoriesIds);

                ViewBag.Manufacturers = await BuildManufacturersSelecetListItems(model.ManufacturersIds);

                return View(model);
            }


            try
            {
                var requestOptions = ObjectMapper.Map<ProductModel, ProductRequestOptions>(model);

                await _productService.CreateAsync(requestOptions);

                return RedirectToAction("Index");

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }
        }

        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productService.GetAsync(id);

            var model = ObjectMapper.Map<Product, ProductModel>(product);

            ViewBag.Product = ObjectMapper.Map<Product, ProductVM>(product);

            ViewBag.Categories = await BuildCategoriesSelecetListItems(model.CategoriesIds);

            ViewBag.Manufacturers = await BuildManufacturersSelecetListItems(model.ManufacturersIds);

            return View(model);
        }

        [HttpPost]
        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            if (!ModelState.IsValid)
            {

                var product = await _productService.GetAsync(model.Id);

                ViewBag.Categories = await BuildCategoriesSelecetListItems(model.CategoriesIds);

                ViewBag.Manufacturers = await BuildManufacturersSelecetListItems(model.ManufacturersIds);

                ViewBag.Product = ObjectMapper.Map<Product, ProductVM>(product);

                return View(model);
            }


            try
            {
                var requestOptions = ObjectMapper.Map<ProductModel, ProductRequestOptions>(model);

                await _productService.UpdateAsync(model.Id, requestOptions);

                return RedirectToAction("Index");

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ListProductImages(string id, ListProductImagesModel model)
        {
            var productImages = await _productService.ListProductImageAsync(id);

            var result = ObjectMapper.Map<List<ProductImage>, List<ProductImageVM>>(productImages);

            model.Data = result;

            return Json(model);
        }

        [RuleSetForClientSideMessages("*")]
        public IActionResult CreateProductImageModal(string productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return PartialView("_Create.MultiMedia.Image", new CreateProductImageModel { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageModel model)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            string imageName = string.Format("{0}{1}", Guid.NewGuid().ToString(), Path.GetExtension(model.Image.FileName));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await model.Image.CopyToAsync(memoryStream);

                await _blobContainer.SaveAsync(imageName, memoryStream.ToArray());
            }

            var requestOptions = new ProductImageRequestCreateOptions
            {
                Image = HttpContext.GenerateFileLink(imageName),
                DisplayOrder = model.DisplayOrder
            };

            var result = await _productService.CreateProductImageAsync(model.ProductId, requestOptions);

            return Json(ObjectMapper.Map<Product, ProductVM>(result));
        }

        [RuleSetForClientSideMessages("*")]
        public async Task<IActionResult> EditProductImageModal(string productId, string productImageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.ListProductImageAsync(productId);

            var productImage = result.SingleOrDefault(x => x.Id == productImageId);

            if (productImage == null)
            {
                return BadRequest("product image is not exist");
            }

            var model = new UpdateProductImageModel
            {
                ProductId = productImage.ProductId,
                ProductImageId = productImage.Id,
                DisplayOrder = productImage.DisplayOrder
            };

            return PartialView("_Edit.MultiMedia.Image", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductImage(UpdateProductImageModel model)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            var requestOptions = new ProductImageRequestUpdateOptions
            {
                DisplayOrder = model.DisplayOrder
            };

            var result = await _productService.UpdateProductImageAsync(model.ProductId, model.ProductImageId, requestOptions);

            return Json(ObjectMapper.Map<Product, ProductVM>(result));

        }


        [HttpPost]
        public async Task<IActionResult> RemoveProductImage([FromBody] RemoveProductImageModel model)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            var result = await _productService.DeleteProductImageAsync(model.ProductId, model.ProductImageId);

            return Json(ObjectMapper.Map<Product, ProductVM>(result));

        }

        private async Task<List<SelectListItem>> BuildCategoriesSelecetListItems(string[]? categoriesValues = null , bool idDefaultValue  = true)
        {
            var categorySelectItems = new List<SelectListItem>();

            var categories = await _categoryService.ListAsync(new CategoryListRequestOptions { Desc = true });


            if (categories != null)
            {
                if (idDefaultValue)
                {
                    categorySelectItems = categories
                        .Select(x => new SelectListItem 
                        { 
                            Text = x.Name, 
                            Value = x.Id, 
                            Selected = categoriesValues?.Contains(x.Id) ?? false

                        }).ToList();

                }
                else
                {
                    categorySelectItems = categories
                     .Select(x => new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Name,                      
                         Selected = categoriesValues?.Contains(x.Name) ?? false

                     }).ToList();
                }
               

            }

            return categorySelectItems;
        }


        private async Task<List<SelectListItem>> BuildManufacturersSelecetListItems(string[]? manufacturersValues = null , bool idDefaultValue = true)
        {
            var manufacturerSelectItems = new List<SelectListItem>();

            var manufacturers = await _manufacturerService.ListAsync(new ManufacturerListRequestOptions { Desc = true });


            if (manufacturers != null)
            {
                if (idDefaultValue)
                {
                    manufacturerSelectItems = manufacturers
                        .Select(x => new SelectListItem 
                        { 
                            Text = x.Name, 
                            Value = x.Id, 
                            Selected = manufacturersValues?.Contains(x.Id) ?? false 
                        }).ToList();
                }
                else
                {
                    manufacturerSelectItems = manufacturers
                     .Select(x => new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Name,
                         Selected = manufacturersValues?.Contains(x.Name) ?? false
                     }).ToList();
                }

             
            }

            return manufacturerSelectItems;
        }
    }
}
