﻿namespace MicroStore.Bff.Shopping.Models.Catalog.Categories
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryModel()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
