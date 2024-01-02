using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.Manufacturers;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.Manufacturers
{
    public class ManufacturerCommandServiceTest : ManufacturerCommandTestBase
    {
        protected IManufacturerCommandService ManufacturerCommandService { get; }

        public ManufacturerCommandServiceTest()
        {
            ManufacturerCommandService = GetRequiredService<IManufacturerCommandService>();
        }

        [Test]
        public async Task Should_create_manufacturer()
        {
            var model = new ManufacturerModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await ManufacturerCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var manufacturer = await SingleAsync<Manufacturer>(x => x.Id == result.Value.Id);

                manufacturer.AssertManufacturerModel(model);

                Assert.That(await TestHarness.Published.Any<EntityCreatedEvent<ManufacturerEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityCreatedEvent<ManufacturerEto>>());

                var elasticManufacturer = await FindElasticDoc<ElasticManufacturer>(manufacturer.Id);

                elasticManufacturer.Should().NotBeNull();

                elasticManufacturer!.AssertElasticManufacturer(manufacturer);
            });

        }

        [Test]
        public async Task Should_return_failure_result_when_manufacturer_model_not_valid_while_creating_manufacturer()
        {

            var fakeManufacturer = await CreateManufacturer();

            var model = new ManufacturerModel
            {
                Name = fakeManufacturer.Name,
                Description = fakeManufacturer.Description
            };

            var result = await ManufacturerCommandService.CreateAsync(model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_update_manufacturer()
        {
            var fakeManufacturer = await CreateManufacturer();

            var model = new ManufacturerModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await ManufacturerCommandService.UpdateAsync(fakeManufacturer.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var manufacturer = await SingleAsync<Manufacturer>(x => x.Id == fakeManufacturer.Id);

                manufacturer.AssertManufacturerModel(model);

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ManufacturerEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ManufacturerEto>>());

                var elasticManufacturer = await FindElasticDoc<ElasticManufacturer>(manufacturer.Id);

                elasticManufacturer.Should().NotBeNull();

                elasticManufacturer!.AssertElasticManufacturer(manufacturer);

            });


        }

        [Test]
        public async Task Should_return_failure_result_when_manufacturer_is_not_exist_while_updating_manufacturer()
        {
            var model = new ManufacturerModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };


            var result = await ManufacturerCommandService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }



        [Test]
        public async Task Should_return_failure_result_when_manufacturer_is_not_valid_while_updating_manufacturer()
        {
            var fakeManufacturer1 = await CreateManufacturer();
            var fakeManufacturer2 = await CreateManufacturer();

            var model = new ManufacturerModel
            {
                Name = fakeManufacturer2.Name,
                Description = fakeManufacturer2.Description
            };

            var result = await ManufacturerCommandService.UpdateAsync(fakeManufacturer1.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();

        }

    }

    public class ManufacturerCommandTestBase : BaseTestFixture
    {
        public async Task<Manufacturer> CreateManufacturer()
        {
            var manufacturer = new Manufacturer
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            await Insert(manufacturer);

            return manufacturer;
        }
    }
}
