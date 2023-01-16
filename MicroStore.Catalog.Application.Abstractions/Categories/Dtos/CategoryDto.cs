﻿using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class CategoryDto : EntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 

    }
}
