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
        public List<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();


        public Product()
        {
            Id = Guid.NewGuid();
        }


     
          
        public void AssignProductImage(string imageUrl , int displayorder)
        {
            ProductImages.Add(new ProductImage
            {
                ImagePath = imageUrl,
                DisplayOrder = displayorder
            });
        }

        public void RemoveProductImage(Guid productImageid)
        {
            ProductImage productImage = ProductImages.Single(x => x.Id == productImageid);

            ProductImages.Remove(productImage);         
        }



    }
}
