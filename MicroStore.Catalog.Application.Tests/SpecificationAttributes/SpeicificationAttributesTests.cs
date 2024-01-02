using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.SpecificationAttributes;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.SpecificationAttributes
{
    public class SpeicificationAttributesTests : BaseTestFixture
    {
        private readonly ISpecificationAttributeApplicationService _sut;

        public SpeicificationAttributesTests()
        {
            _sut = GetRequiredService<ISpecificationAttributeApplicationService>();
        }


        [Test]
        public async Task Should_create_specification_attribute()
        {
            var model = CreateFakeSpecificationAttributeModel();

            var result = await _sut.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async (val) =>
            {
                var attribute = await SingleAsync<SpecificationAttribute>(x => x.Id == val.Id, x => x.Options);

                attribute.AssertSpecificationAttributeModel(model);

                Assert.That(await TestHarness.Published.Any<EntityCreatedEvent<SpecificationAttributeEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityCreatedEvent<SpecificationAttributeEto>>());

                var elasticSpecificationAttribute = await FindElasticDoc<ElasticSpecificationAttribute>(attribute.Id);

                elasticSpecificationAttribute.Should().NotBeNull();

                elasticSpecificationAttribute!.AssertElasticSpecificationAttribute(attribute);
            });


        }

        [Test]
        public async Task Should_return_failure_result_when_creating_specification_attribute_while_name_is_already_exist()
        {
            var model = CreateFakeSpecificationAttributeModel();
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            model.Name = fakeAttribute.Name;

            var result=  await _sut.CreateAsync(model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }


        [Test]
        public async Task Should_update_specification_attribute()
        {
            var model = CreateFakeSpecificationAttributeModel();

            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var result = await _sut.UpdateAsync(fakeAttribute.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async (val) =>
            {
                var attribute = await SingleAsync<SpecificationAttribute>(x => x.Id == val.Id, x => x.Options);

                attribute.AssertSpecificationAttributeModel(model);

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());

                var elasticSpecificationAttribute = await FindElasticDoc<ElasticSpecificationAttribute>(val.Id);

                elasticSpecificationAttribute.Should().NotBeNull();

                elasticSpecificationAttribute!.AssertElasticSpecificationAttribute(attribute);
            });

      
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_specification_attribute_while_Id_is_not_exist()
        {
            var specificationAttributeId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeModel();

            var result = await _sut.UpdateAsync(specificationAttributeId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_specification_attribute_when_name_is_already_exist()
        {
            var firstAttribute = await CreateFakeSpecificationAttribute();
            var secondAttribute = await CreateFakeSpecificationAttribute();

            var model = CreateFakeSpecificationAttributeModel();

            model.Name = secondAttribute.Name;

            var result = await _sut.UpdateAsync(firstAttribute.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_specification_attribute()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var result = await _sut.RemoveAsync(fakeAttribute.Id);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var attribute = await SingleOrDefaultAsync<SpecificationAttribute>(x => x.Id == fakeAttribute.Id);

                attribute.Should().BeNull();

                Assert.That(await TestHarness.Published.Any<EntityDeletedEvent<SpecificationAttributeEto>>());

                var elasticSpecificationAttribute = await FindElasticDoc<ElasticSpecificationAttribute>(fakeAttribute.Id);

                elasticSpecificationAttribute.Should().BeNull();
            });


        }

        [Test]
        public async Task Should_return_failure_result_when_removing_specification_attribute_while_id_is_not_exist()
        {
            var attributeId = Guid.NewGuid().ToString();

            var result = await _sut.RemoveAsync(attributeId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task should_create_specification_attribute_option()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.CreateOptionAsync(fakeAttribute.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var attibute = await SingleAsync<SpecificationAttribute>(x => x.Id == fakeAttribute.Id, x => x.Options);

                var option = attibute.Options.Single(x => x.Name == model.Name);

                option.AssertSpecificationAttributeOptionModel(model);

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());
            });    
        }



        public async Task Should_return_failure_result_when_create_specification_attribute_option_when_attribute_is_not_exist()
        {
            var attributeId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result=  await _sut.CreateOptionAsync(attributeId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
        public async Task Should_return_failure_result_when_create_specification_attribute_option_when_attribute_name_exist()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var model = CreateFakeSpecificationAttributeOptionModel();

            model.Name = fakeAttribute.Options.Last().Name;

            var result = await _sut.CreateOptionAsync(fakeAttribute.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task should_update_specification_attribute_option()
        {
            var fakeAttribute=  await CreateFakeSpecificationAttribute();

            var optionId = fakeAttribute.Options.First().Id;

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.UpdateOptionAsync(fakeAttribute.Id, optionId, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async _ =>
            {
                var attribute = await SingleAsync<SpecificationAttribute>(x => x.Id == fakeAttribute.Id, x => x.Options);

                var option = attribute.Options.Single(x => x.Id == optionId);

                option.AssertSpecificationAttributeOptionModel(model);

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());
            });


        }

        [Test]
        public async Task Should_return_failure_result_when_updating_specification_attribute_option_while_attribute_is_not_exist()
        {
            var attributeId = Guid.NewGuid().ToString();

            var optionId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.UpdateOptionAsync(attributeId, optionId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_specification_attribute_option_while_option_is_not_exist()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var optionId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.UpdateOptionAsync(fakeAttribute.Id, optionId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_specification_attribute_option_while_option_name_dublicated()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var optionId = fakeAttribute.Options.First().Id;

            var model = CreateFakeSpecificationAttributeOptionModel();

            model.Name = fakeAttribute.Options.Last().Name;

            var result = await _sut.UpdateOptionAsync(fakeAttribute.Id, optionId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_specification_attribute_option()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var optionId = fakeAttribute.Options.First().Id;

            var result = await _sut.RemoveOptionAsync(fakeAttribute.Id, optionId);

            result.IsSuccess.Should().BeTrue();


            await result.IfSuccess(async _ =>
            {
                var attribute = await SingleAsync<SpecificationAttribute>(x => x.Id == fakeAttribute.Id, x => x.Options);

                var option = attribute.Options.SingleOrDefault(x => x.Id == optionId);

                option.Should().BeNull();

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<SpecificationAttributeEto>>());
            });
   
        }

        [Test]
        public async Task Should_return_failure_result_when_removing_specification_attribute_option_while_attribute_is_not_exist()
        {
            var attributeId = Guid.NewGuid().ToString();

            var optionId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.RemoveOptionAsync(attributeId, optionId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_removing_specification_attribute_option_while_option_is_not_exist()
        {
            var fakeAttribute = await CreateFakeSpecificationAttribute();

            var optionId = Guid.NewGuid().ToString();

            var model = CreateFakeSpecificationAttributeOptionModel();

            var result = await _sut.RemoveOptionAsync(fakeAttribute.Id, optionId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        private SpecificationAttributeModel CreateFakeSpecificationAttributeModel()
        {
            return new SpecificationAttributeModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Options = new HashSet<SpecificationAttributeOptionModel>
                {
                    new SpecificationAttributeOptionModel
                    {
                        Name = Guid.NewGuid().ToString()
                    },


                    new SpecificationAttributeOptionModel
                    {
                        Name = Guid.NewGuid().ToString()
                    },

                    new SpecificationAttributeOptionModel
                    {
                        Name = Guid.NewGuid().ToString()
                    },
                }

            };
        }

        private async Task<SpecificationAttribute> CreateFakeSpecificationAttribute()
        {
            SpecificationAttribute specificationAttribute = new SpecificationAttribute
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Options=  new List<SpecificationAttributeOption>
                {
                    new SpecificationAttributeOption
                    {
                        Name = Guid.NewGuid().ToString(),
                    },


                    new SpecificationAttributeOption
                    {
                        Name = Guid.NewGuid().ToString(),
                    },


                    new SpecificationAttributeOption
                    {
                        Name = Guid.NewGuid().ToString(),
                    },
                }

            };

            await Insert(specificationAttribute);

            return specificationAttribute;
        }

        private SpecificationAttributeOptionModel CreateFakeSpecificationAttributeOptionModel()
        {
            return new SpecificationAttributeOptionModel
            {
                Name = Guid.NewGuid().ToString()
            };
        }
    }
}
