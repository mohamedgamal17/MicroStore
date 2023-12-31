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
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MimeMapping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]

    public class ProductController : AdministrationController
    {

        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;

        private readonly ILogger<ProductController> _logger;

        private readonly IObjectStorageProvider _objectStorageProvider;

        private readonly ManufacturerService _manufacturerService;

        private readonly ProductAnalysisService _productAnalysisService;

        private readonly ProductImageService _productImageService;
        public ProductController(ProductService productService, CategoryService categoryService, ILogger<ProductController> logger, IObjectStorageProvider objectStorageProvider, ManufacturerService manufacturerService, ProductAnalysisService productAnalysisService, ProductImageService productImageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
            _objectStorageProvider = objectStorageProvider;
            _manufacturerService = manufacturerService;
            _productAnalysisService = productAnalysisService;
            _productImageService = productImageService;
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
                Length = model.Length,
                SortBy = "creation",
                Desc = true
            };

            var response = await _productService.ListAsync(requestOptions);

            var responseModel = new ProductListModel
            {
                Start = model.Start,
                Length = model.Length,
                Draw = model.Draw,
                RecordsTotal = response.TotalCount,
                Data = ObjectMapper.Map<List<Product>, List<ProductVM>>(response.Items)
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

                var response =  await _productService.CreateAsync(requestOptions);

                NotificationManager.Success("Product has been created successfully !");

                return RedirectToAction("Edit" , new { id= response.Id });

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Error.MapToModelState(ModelState);

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

                NotificationManager.Success("Product has been updated successfully !");

                return RedirectToAction("Edit", new {id = model.Id });

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Error.MapToModelState(ModelState);

                ViewBag.Categories = await BuildCategoriesSelecetListItems();

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ListProductImages(string id, ListProductImagesModel model)
        {
            var productImages = await _productImageService.ListAsync(id);

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

                memoryStream.Seek(0, SeekOrigin.Begin);

                var args = new S3ObjectSaveArgs
                {
                    Name = imageName,
                    Data = memoryStream,
                    ContentType = MimeUtility.GetMimeMapping(model.Image.FileName)
                };

                await _objectStorageProvider.SaveAsync(args);
            }

            var requestOptions = new ProductImageRequestptions
            {
                Image =  await _objectStorageProvider.CalculatePublicReferenceUrl(imageName),
                DisplayOrder = model.DisplayOrder
            };

            var result = await _productImageService.CreateAsync(model.ProductId, requestOptions);

            return Json(ObjectMapper.Map<ProductImage, ProductImageVM>(result));
        }

        [HttpGet]
        public async Task<IActionResult> EditProductImageModal(string productId, string productImageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productImage = await _productImageService.GetAsync(productId,productImageId);

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

            try
            {
                var productImage = await _productImageService.GetAsync(model.ProductId, model.ProductImageId);

                var requestOptions = new ProductImageRequestptions
                {
                    Image = productImage.Image,
                    DisplayOrder = model.DisplayOrder
                };

                var result = await _productImageService.UpdateAsync(model.ProductId, model.ProductImageId, requestOptions);

                return Json(ObjectMapper.Map<ProductImage, ProductImageVM>(result));

            }catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Product image is not exist");
            }       
        }


        [HttpPost]
        public async Task<IActionResult> RemoveProductImage([FromBody] RemoveProductImageModel model)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            await _productImageService.DeleteAsync(model.ProductId, model.ProductImageId);

            return NoContent();

        }

        public async Task<IActionResult> SalesReport(string id)
        {
            var response = await _productService.GetAsync(id);

            ViewBag.Product = ObjectMapper.Map<Product, ProductVM>(response);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SalesReport(string id, ProductSalesReportModel model)
        {
            var requestOptions = new ProductSalesReportRequestOptions
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Period = model.GroupBy,
                Length = model.Length,
                Skip = model.Skip
            };

            var response = await _productAnalysisService.GetProductSalesReport(id,requestOptions);

            var responseModel = new ProductSalesReportModel
            {
                Data = ObjectMapper.Map<List<ProductSalesReport>, List<ProductSalesReportVM>>(response.Items),
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                GroupBy = model.GroupBy,
                Length = response.Lenght,
                Start = response.Skip,
                Draw = model.Draw,
                RecordsTotal = response.TotalCount
            };

            return Ok(responseModel);
        }

        public async Task<IActionResult> SalesUnitForecasting(string id)
        {
            try
            {
                var productResponse = await _productService.GetAsync(id);

                ViewBag.Product = ObjectMapper.Map<Product, ProductVM>(productResponse);

                var lastYearDate = DateTime.Now.AddMonths(-12);

                var startDate = new DateTime(lastYearDate.Year, lastYearDate.Month, 1);

                var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                var forecastRequestOptions = new ForecastRequestOptions
                {
                    ConfidenceLevel = 0.95f,
                    Horizon = 6
                };

                var forecasting = await _productAnalysisService.Forecast(id, forecastRequestOptions);

                var reportRequestOptions = new ProductSalesReportRequestOptions
                {
                    StartDate = startDate,
                    EndDate =endDate,
                    Period = ReportPeriod.Monthly,
                    Length = 12,
                    Skip = 0
                };

                var monthlySalesReport = await _productAnalysisService.GetProductSalesReport(id, reportRequestOptions);

                var projection = monthlySalesReport.Items.OrderBy(x => x.Date).Select(x => new ProductSalesChartDataModel
                {
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                    Date = x.Date.ToString("MM-dd-yyyy"),

                }).ToList();

                int monthOffset = 1;

                for (int i = 0; i < forecasting.ForecastedValues.Length; i++)
                {

                    projection.Add(new ProductSalesChartDataModel
                    {
                        Quantity = forecasting.ForecastedValues[i],
                        Date = DateTime.Now.AddMonths(monthOffset).ToString("MM-dd-yyyy"),
                        IsForecasted = true
                    });

                    monthOffset++;
                }

                return View();
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                NotificationManager.Error(ex.Error.Detail);

                return RedirectToAction(nameof(SalesReport), new { id = id });
            }

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
