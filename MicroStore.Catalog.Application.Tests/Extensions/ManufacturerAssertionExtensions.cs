using FluentAssertions;
using MicroStore.Catalog.Application.Models.Manufacturers;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class ManufacturerAssertionExtensions
    {
        public static void AssertManufacturerModel(this Manufacturer manufacturer, ManufacturerModel model)
        {
            manufacturer.Name.Should().Be(model.Name);
            manufacturer.Description.Should().Be(model.Description);
        }

        public static void AssertElasticManufacturer(this ElasticManufacturer elasticManufacturer, Manufacturer manufacturer)
        {
            elasticManufacturer.Id.Should().Be(manufacturer.Id);
            elasticManufacturer.Name.Should().Be(manufacturer.Name);
            elasticManufacturer.Description.Should().Be(manufacturer.Description);
        }

        public static void AssertElasticProductManufacturer(this ElasticProductManufacturer productManufacturer, Manufacturer manufacturer)
        {
            productManufacturer.Id.Should().Be(manufacturer.Id);
            productManufacturer.Name.Should().Be(manufacturer.Name);

        }

    }
}
