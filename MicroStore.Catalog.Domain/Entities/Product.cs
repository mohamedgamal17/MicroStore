
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Domain.Events;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class Product : BasicAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Sku { get; private set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; private set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }

        private List<ProductCategory> _productCategories = new List<ProductCategory>();
        public IReadOnlyList<ProductCategory> ProductCategories => _productCategories.AsReadOnly();

        public Product(string sku, string name, decimal price)
            : base(Guid.NewGuid())
        {
            Sku = sku;
            Name = name;
            Price = price;
            AddLocalEvent(new CreateProductEvent(Id, Name, Sku, price));
        }

        protected Product() // Require For EFCore
        {

        }

        public void AdjustProductName(string name)
        {
            if (Name == name || string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            Name = name;

            AddLocalEvent(new AdjustProductNameEvent(Id, Name));
        }


        public void AdjustProductSku(string sku)
        {
            if (Sku == sku || string.IsNullOrWhiteSpace(sku))
            {
                return;
            }

            Sku = sku;

            AddLocalEvent(new AdjustProductSkuEvent(Id, Sku));
        }


        public void AdjustProductPrice(decimal price)
        {
            if (Price == price)
            {
                return;
            }

            Price = price;

            AddLocalEvent(new AdjustProductPriceEvent(Id, Price));
        }

        public void SetProductShortDescription(string shortDescription)
        {
            if (shortDescription.IsNullOrEmpty() ||
                ShortDescription == shortDescription)
            {
                return;
            }

            ShortDescription = shortDescription;
        }


        public void SetProductLongDescription(string longDescription)
        {
            if (longDescription.IsNullOrEmpty() ||
                LongDescription == longDescription)
            {
                return;
            }


            LongDescription = longDescription;
        }



        public void SetProductOldPrice(decimal oldPrice)
        {

            if (OldPrice == oldPrice)
            {
                return;
            }

            OldPrice = oldPrice;
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
            Result result = CanRemoveProductCategory(categoryId);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error); // Fail fast princible
            }


            ProductCategory productCategory = _productCategories.Single(x => x.Id == categoryId);

            _productCategories.Remove(productCategory);
        }




        public Result CanRemoveProductCategory(Guid categoryId)
        {
            bool isProductCategoryExist = _productCategories.Any(x => x.CategoryId == categoryId);

            return isProductCategoryExist ? Result.Success()
                : Result.Failure("Current product is not mapped to specified category");
        }



    }
}
