﻿

namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class CategoryListDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductByCategoryDto> Products { get; set; }
    }
}