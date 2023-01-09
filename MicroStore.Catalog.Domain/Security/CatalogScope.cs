namespace MicroStore.Catalog.Domain.Security
{
    public static class CatalogScope
    {
        public static class Product
        {
            public readonly static string List = "catalog.product.list";

            public readonly static string Read = "catalog.product.read";

            public readonly static string Create = "catalog.product.create";

            public readonly static string Update = "catalog.product.update";
        }

        public static class Category
        {
            public readonly static string List = "catalog.category.list";

            public readonly static string Read = "catalog.category.read";

            public readonly static string Create = "catalog.category.create";

            public readonly static string Update = "catalog.category.update";
        }

        public static class ProductCategory
        {
            public readonly static string Create = "catalog.productcategory.create";

            public readonly static string Update = "catalog.productcategory.update";

            public readonly static string Delete = "catalog.productcategory.delete";
        }

        public static class ProductImage
        {
            public readonly static string Create = "catalog.productimage.create";

            public readonly static string Update = "catalog.productimage.update";

            public readonly static string Delete = "catalog.productimage.delete";
        }
    }
}
