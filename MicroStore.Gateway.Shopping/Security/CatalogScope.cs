namespace MicroStore.Gateway.Shopping.Security
{
    public static class CatalogScope
    {
        public static List<string> List()
        {
            return new List<string>
            {
                Product.List,
                Product.Read,
                Product.Create,
                Product.Update,
                Product.Update,
                Category.List,
                Category.Read,
                Category.Update,
                ProductCategory.Create,
                ProductCategory.Update,
                ProductCategory.Delete,
                ProductImage.Create,
                ProductImage.Update,
                ProductImage.Delete,
            };
        }

        public static class Product
        {
            public const string List = "catalog.product.list";

            public const string Read = "catalog.product.read";

            public const string Create = "catalog.product.create";

            public const string Update = "catalog.product.update";
        }

        public static class Category
        {
            public const string List = "catalog.category.list";

            public const string Read = "catalog.category.read";

            public const string Create = "catalog.category.create";

            public const string Update = "catalog.category.update";
        }

        public static class ProductCategory
        {
            public const string Create = "catalog.productcategory.create";

            public const string Update = "catalog.productcategory.update";

            public const string Delete = "catalog.productcategory.delete";
        }

        public static class ProductImage
        {
            public const string Create = "catalog.productimage.create";

            public const string Update = "catalog.productimage.update";

            public const string Delete = "catalog.productimage.delete";
        }
    }
}
