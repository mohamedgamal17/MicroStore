﻿namespace MicroStore.Bff.Shopping.Models.Catalog.Products
{
    public class ProductImageModel
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }

        public ProductImageModel()
        {
            Image = string.Empty;
            DisplayOrder = 0;
        }
    }
}
