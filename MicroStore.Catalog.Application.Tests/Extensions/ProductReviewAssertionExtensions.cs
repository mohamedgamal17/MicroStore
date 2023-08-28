using FluentAssertions;
using MicroStore.Catalog.Application.Models.ProductReviews;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class ProductReviewAssertionExtensions
    {
        public static void AssertProductReviewModel(this ProductReview productReview, ProductReviewModel model)
        {
            productReview.ReviewText.Should().Be(model.ReviewText);
            productReview.Rating.Should().Be(model.Rating);
            productReview.Title.Should().Be(model.Title);
        }

        public static void AssertCreateProductReviewModel(this ProductReview productReview, CreateProductReviewModel model)
        {
            productReview.AssertProductReviewModel(model);
            productReview.UserId.Should().Be(model.UserId);
        }

        public static void AssertElasticProductReview(this ElasticProductReview elasticProductReview , ProductReview productReview)
        {
            elasticProductReview.Id.Should().Be(productReview.Id);
            elasticProductReview.Title.Should().Be(productReview.Title);
            elasticProductReview.ReviewText.Should().Be(productReview.ReviewText);
            elasticProductReview.ReplayText.Should().Be(productReview.ReplayText);
            elasticProductReview.Rating.Should().Be(productReview.Rating);
            elasticProductReview.UserId.Should().Be(productReview.UserId);
        }

    }
}
