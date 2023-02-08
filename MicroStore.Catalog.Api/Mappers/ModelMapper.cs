using AutoMapper;
using MicroStore.Catalog.Api.Models;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Products;

namespace MicroStore.Catalog.Api.Mappers
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<ProductModel, CreateProductCommand>();

            CreateMap<ProductModel, UpdateProductCommand>();

            CreateMap<CategoryModel, CreateCategoryCommand>();

            CreateMap<CategoryModel, UpdateCategoryCommand>();

            CreateMap<ProductImageModel, CreateProductImageCommand>();
     
        }
    }
}
