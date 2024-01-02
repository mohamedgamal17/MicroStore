using FluentAssertions;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Application.Abstractions.ProductReveiws;

namespace MicroStore.Catalog.Application.Tests.ProductReviews
{
    public class ProductReviewService_Tests : BaseTestFixture
    {
        private readonly IProductReviewService _sut;
        public ProductReviewService_Tests()
        {
            _sut = GetRequiredService<IProductReviewService>();
        }

        [Test]
        public async Task Should_create_product_review()
        {
            var fakeProduct = await GenerateProduct();

            var model = GenerateCreateProductReviewModel();

            var result =  await _sut.CreateAsync(fakeProduct.Id, model);

            result.IsSuccess.Should().BeTrue();


            await result.IfSuccess(async val =>
            {
                var productReview = await SingleAsync<ProductReview>(x => x.Id == val.Id && x.ProductId == fakeProduct.Id);

                productReview.AssertCreateProductReviewModel(model);

                Assert.That(await TestHarness.Published.Any<EntityCreatedEvent<ProductReviewEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityCreatedEvent<ProductReviewEto>>());

                var elasticEntity = await FindElasticDoc<ElasticProductReview>(val.Id);

                elasticEntity.Should().NotBeNull();

                elasticEntity!.AssertElasticProductReview(productReview);
            });
;
        }

        [Test]
        public async Task Should_return_failure_result_while_creating_product_review_while_product_is_not_found()
        {
            var productId = Guid.NewGuid().ToString();

            var model = GenerateCreateProductReviewModel();

            var result = await _sut.CreateAsync(productId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_update_product_review()
        {
            var fakeProductReview = await GenerateProductReview();

            var model = GenerateProductReviewModel();

            
            var result = await _sut.UpdateAsync(fakeProductReview.ProductId, fakeProductReview.Id, model);

            result.IsSuccess.Should().BeTrue();

            await result.IfSuccess(async val =>
            {
                var productReview = await SingleAsync<ProductReview>(x => x.Id == val.Id && x.ProductId == fakeProductReview.ProductId);

                productReview.AssertProductReviewModel(model);

                Assert.That(await TestHarness.Published.Any<EntityUpdatedEvent<ProductReviewEto>>());

                Assert.That(await TestHarness.Consumed.Any<EntityUpdatedEvent<ProductReviewEto>>());

                var elasticEntity = await FindElasticDoc<ElasticProductReview>(val.Id);

                elasticEntity.Should().NotBeNull();

                elasticEntity!.AssertElasticProductReview(productReview);
            });


        }

        [Test]
        public async Task Should_return_failure_result_while_updating_product_review_when_product_is_not_exist()
        {
            var productId = Guid.NewGuid().ToString();

            var productReviewId = Guid.NewGuid().ToString();

            var model = GenerateProductReviewModel();

            var result = await _sut.UpdateAsync(productId, productReviewId, model); 

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_updating_product_review_when_product_review_is_not_exist()
        {
            var Product = await GenerateProduct();

            var productReviewId = Guid.NewGuid().ToString();

            var model = GenerateProductReviewModel();

            var result = await _sut.UpdateAsync(Product.Id, productReviewId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_replay_on_product_review()
        {
            var fakeProductReview = await GenerateProductReview();

            var model = GenerateProductReviewReplayModel();

            var result = await _sut.ReplayAsync(fakeProductReview.ProductId, fakeProductReview.Id, model);

            result.IsSuccess.Should().BeTrue();


            var productReview = await SingleAsync<ProductReview>(x => x.Id == result.Value.Id && x.ProductId == fakeProductReview.ProductId);

            productReview.ReplayText.Should().Be(model.ReplayText);

        }


        [Test]
        public async Task Should_return_failure_result_while_replaying_on_product_review_when_product_is_not_exist()
        {
            var productId = Guid.NewGuid().ToString();

            var productReviewId = Guid.NewGuid().ToString();

            var model = GenerateProductReviewReplayModel();

            var result = await _sut.ReplayAsync(productId, productReviewId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_replaying_on_product_review_when_product_review_is_not_exist()
        {
            var Product = await GenerateProduct();

            var productReviewId = Guid.NewGuid().ToString();

            var model = GenerateProductReviewReplayModel();

            var result = await _sut.ReplayAsync(Product.Id, productReviewId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_remove_product_review()
        {
            var fakeProductReview = await GenerateProductReview();

            var result = await _sut.DeleteAsync(fakeProductReview.ProductId, fakeProductReview.Id);

            result.IsSuccess.Should().BeTrue();

            var productReview = await SingleOrDefaultAsync<ProductReview>(x => x.Id == fakeProductReview.Id && x.ProductId == fakeProductReview.ProductId);

            productReview.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_while_removing_product_review_when_product_is_not_exist()
        {
            var productId = Guid.NewGuid().ToString();

            var productReviewId = Guid.NewGuid().ToString();

            var model = GenerateProductReviewReplayModel();

            var result = await _sut.ReplayAsync(productId, productReviewId, model);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_return_failure_result_while_removing_product_review_when_product_review_is_not_exist()
        {
            var Product = await GenerateProduct();

            var productReviewId = Guid.NewGuid().ToString();

            var result = await _sut.DeleteAsync(Product.Id, productReviewId);

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }


        private async Task<Product> GenerateProduct()
        {
            Product product = new Product
            {
                Name = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 65
            };


            await Insert(product);

            return product;
        }

        private async Task<ProductReview> GenerateProductReview()
        {
            var product = await GenerateProduct();

            var productReview = new ProductReview
            {
                ProductId = product.Id,
                ReviewText = Guid.NewGuid().ToString(),
                ReplayText = Guid.NewGuid().ToString(),
                Rating = 5,
                UserId = Guid.NewGuid().ToString(),
                Title = Guid.NewGuid().ToString()
            };

            await Insert(productReview);

            return productReview;
        }

        private CreateProductReviewModel GenerateCreateProductReviewModel()
        {
            return new CreateProductReviewModel
            {
                ReviewText = Guid.NewGuid().ToString(),
                Rating = 4,
                Title = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
            };
        }

        private ProductReviewModel GenerateProductReviewModel()
        {
            return new ProductReviewModel
            {
                ReviewText = Guid.NewGuid().ToString(),
                Rating = 4,
                Title = Guid.NewGuid().ToString(),
            };
        }

        private ProductReviewReplayModel GenerateProductReviewReplayModel()
        {
            return new ProductReviewReplayModel
            {
                ReplayText = Guid.NewGuid().ToString()
            };
        }

    }
}
