using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Catalog
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductVM>()
                .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.Images))
                .ForMember(x => x.Categories, opt => opt.MapFrom(c => c.Categories))
                .ForMember(x => x.Dimension, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.Images));

            
            CreateMap<PagedList<Product>, PagedList<ProductVM>>();

            CreateMap<Product, ProductModel>()
                .ForMember(x => x.CategoriesIds, opt => opt.MapFrom(c => c.Categories.Select(x => x.CategoryId).ToArray()))
                .ForMember(x => x.ManufacturersIds, opt => opt.MapFrom(c => c.Manufacturers.Select(x => x.ManufacturerId).ToArray()));



            CreateMap<ProductModel,ProductRequestOptions >()
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.CategoriesIds, opt => opt.MapFrom(c => c.CategoriesIds))
                .ForMember(x => x.ManufacturersIds, opt => opt.MapFrom(c => c.ManufacturersIds));


            CreateMap<ProductImage, ProductImageVM>();

            CreateMap<ProductCategory, ProductCategoryVM>();

            CreateMap<ProductSalesReport, ProductSalesReportVM>();

        }
    }
}
