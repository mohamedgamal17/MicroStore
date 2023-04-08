using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Manufacturers;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Tests.Manufacturers
{
    public class ManufacturerQueryServiceTest : BaseTestFixture
    {
        protected IManufacturerQueryService ManufacturerQueryService { get; }

        public ManufacturerQueryServiceTest()
        {
            ManufacturerQueryService = GetRequiredService<IManufacturerQueryService>();
        }

        [Test]
        public async Task Should_get_all_manufacturers()
        {
            var result = await ManufacturerQueryService.ListAsync(new SortingQueryParams());

            result.IsSuccess.Should().BeTrue();

            result.Value.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task Should_get_manufacturer_by_id()
        {
            var manufacturerId = ( await First<Manufacturer>() ).Id;

            var result = await ManufacturerQueryService.GetAsync(manufacturerId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(manufacturerId);
        }
    }
}
