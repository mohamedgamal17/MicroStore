using MicroStore.Catalog.Domain.Events;
using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class Product : BasicAggregateRoot<Guid>
    {
        public string Name { get;  set; }
        public string Sku { get;  set; }
        public string Thumbnail { get; set; } 
        public string ShortDescription { get;set; } = string.Empty;
        public string LongDescription { get;set; } = string.Empty;
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }

        private List<ProductCategory> _productCategories = new List<ProductCategory>();

        private List<ProductImage> _productImages = new List<ProductImage>();
        public IReadOnlyList<ProductCategory> ProductCategories => _productCategories.AsReadOnly();
        public IReadOnlyList<ProductImage> ProductImages => _productImages.AsReadOnly();

    
        public Product(string sku ,string name,double price, string thumbnail)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Thumbnail = thumbnail;
        }

        protected Product() // Require For EFCore
        {

        }
     
        public void AddOrUpdateProductCategory(Category category, bool isFeatured)
        {
            AddOrUpdateProductCategory(category.Id, isFeatured);
        }

        public void AddOrUpdateProductCategory(Guid categoryId, bool isFeatured)
        {
            ProductCategory? productCategory = _productCategories
                .SingleOrDefault(x => x.CategoryId == categoryId);

            if (productCategory == null)
            {
                productCategory = new ProductCategory(categoryId);
                _productCategories.Add(productCategory);
            }

            productCategory.SetFeaturedProduct(isFeatured);
        }


        public void RemoveProductCategory(Category category)
        {
            RemoveProductCategory(category.Id);
        }


        public void RemoveProductCategory(Guid categoryId)
        {
            ProductCategory productCategory = _productCategories.Single(x => x.CategoryId == categoryId);

            _productCategories.Remove(productCategory);
        }

        public void AssignProductImage(string imageUrl , int displayorder)
        {
            _productImages.Add(new ProductImage(imageUrl, displayorder));
        }

        public void UpdateProductImage(Guid productImageId , int displayorder)
        {
            ProductImage productImage =  _productImages.Single(x => x.Id == productImageId);

            productImage.DisplayOrder = displayorder;
        }

        public void RemoveProductImage(Guid productImageid)
        {
            ProductImage productImage = _productImages.Single(x => x.Id == productImageid);

            _productImages.Remove(productImage);         
        }



    }
}
