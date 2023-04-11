﻿using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Dtos
{
    public class ManufacturerDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string  Description { get; set; }
    }
}