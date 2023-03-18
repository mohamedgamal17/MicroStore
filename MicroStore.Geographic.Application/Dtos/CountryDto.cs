﻿#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;

namespace MicroStore.Geographic.Application.Dtos
{
    public class CountryDto : Entity<string>
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public List<StateProvinceDto> StateProvinces { get; set; }
    }
}