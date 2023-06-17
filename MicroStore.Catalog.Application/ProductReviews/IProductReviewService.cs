﻿using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.ProductReviews;

namespace MicroStore.Catalog.Application.ProductReviews
{
    public interface IProductReviewService
    {
        Task<Result<ProductReviewDto>> CreateAsync(string productId, CreateProductReviewModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewDto>> UpdateAsync(string productId, string productReviewId, ProductReviewModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewDto>> ReplayAsync(string productId, string productReviewId, ProductReviewReplayModel model, CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(string productId, string productReviewId, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ProductReviewDto>>> ListAsync(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewDto>> GetAsync(string productId, string productReviewId, CancellationToken cancellationToken = default);
    }
}