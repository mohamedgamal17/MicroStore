﻿using FluentAssertions;
using MicroStore.Catalog.Application.Models.ProductTags;
using MicroStore.Catalog.Application.ProductReviews;
using MicroStore.Catalog.Application.ProductTags;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Tests.ProductTags
{
    public class ProductTagApplicationServiceTests : BaseTestFixture
    {
        private readonly IProductTagApplicationService _sut;

        public ProductTagApplicationServiceTests()
        {
            _sut= GetRequiredService<IProductTagApplicationService>();
        }

        [Test]
        public async Task Should_create_product_tag()
        {
            var model = CreateFakeProductTagModel();

            var result = await _sut.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var productTag = await SingleAsync<ProductTag>(x => x.Id == result.Value.Id);

            productTag.AssertProductTagModel(model);
        }

        [Test]
        public async Task Should_return_failure_result_when_creating_product_tag_while_name_is_alread_exist()
        {
            var model = CreateFakeProductTagModel();

            var fakeProductTag = await CreateFakeProductTag();

            model.Name = fakeProductTag.Name;

            var result = await _sut.CreateAsync(model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_update_product_tag()
        {
            var fakeProductTag = await CreateFakeProductTag();
            var model = CreateFakeProductTagModel();

            var result = await _sut.UpdateAsync(fakeProductTag.Id,model);

            result.IsSuccess.Should().BeTrue();

            var productTag = await SingleAsync<ProductTag>(x => x.Id == fakeProductTag.Id);

            productTag.AssertProductTagModel(model);    
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_product_tag_while_product_tag_is_not_exist()
        {
            var fakeProductTagId = Guid.NewGuid().ToString();

            var model = CreateFakeProductTagModel();

            var result = await _sut.UpdateAsync(fakeProductTagId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_return_failure_result_when_updating_product_tag_while_name_is_already_exist()
        {
            var firstProductTag = await CreateFakeProductTag();
            var secondProductTag = await CreateFakeProductTag();

            var model = CreateFakeProductTagModel();

            model.Name = secondProductTag.Name;

            var result =  await _sut.UpdateAsync(firstProductTag.Id, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<UserFriendlyException>();
        }

        [Test]
        public async Task Should_remove_product_tag()
        {
            var fakeProductTag = await CreateFakeProductTag();

            var result = await _sut.RemoveAsync(fakeProductTag.Id);

            result.IsSuccess.Should().BeTrue();

            var productTag = await SingleOrDefaultAsync<ProductTag>(x => x.Id == fakeProductTag.Id);

            productTag.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_when_removing_product_tag_while_product_tag_is_not_exist()
        {
            var fakeProductTagId = Guid.NewGuid().ToString();

            var result = await _sut.RemoveAsync(fakeProductTagId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        private async Task<ProductTag> CreateFakeProductTag()
        {
            var productTag = new ProductTag
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            await Insert(productTag);

            return productTag;
        }

        private ProductTagModel CreateFakeProductTagModel()
        {
            return new ProductTagModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
        }
    }
}
