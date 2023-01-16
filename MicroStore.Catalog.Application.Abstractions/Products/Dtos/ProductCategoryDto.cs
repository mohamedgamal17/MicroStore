﻿using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductCategoryDto:EntityDto<Guid>
    {
        public Guid CateogryId { get; set; }
        public string Name { get; set; }
        public bool IsFeaturedProduct { get; set; }

    }
}
