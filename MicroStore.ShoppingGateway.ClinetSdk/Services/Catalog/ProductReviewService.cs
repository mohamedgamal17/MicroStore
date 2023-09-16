﻿using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductReviewService
    {
        const string BASE_URL = "/products/{productId}/productreviews";

        const string BASE_URL_WITH_ID = "products/{productId}/productreviews/{reviewId}";

        private readonly MicroStoreClinet _microStoreClient;

        public ProductReviewService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<ProductRevivew> CreateAsync(string productId,ProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

           return await _microStoreClient.MakeRequest<ProductRevivew>(path, HttpMethod.Post, request, cancellationToken);
        }

        public async Task<ProductRevivew> UpdateAsync(string productId,string productReviewId,ProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            var path = string.Format(BASE_URL_WITH_ID, productId, productReviewId);

            return await _microStoreClient.MakeRequest<ProductRevivew>(path,HttpMethod.Put, request, cancellationToken);
        }

        public async Task<ProductRevivew> ReplayAsync(string productId , string reveiwId, ReplayOnProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await _microStoreClient.MakeRequest<ProductRevivew>(path, HttpMethod.Post, request, cancellationToken);
        }


        public async Task<PagedList<ProductRevivew>> ListAsync(string productId, PagingReqeustOptions request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

            return await _microStoreClient.MakeRequest<PagedList<ProductRevivew>>(path, HttpMethod.Get, request ,cancellationToken);
        }


        public async Task<ProductRevivew> GetAsync(string productId ,string reveiwId, CancellationToken cancellationToken)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await _microStoreClient.MakeRequest<ProductRevivew>(path, HttpMethod.Get, cancellationToken: cancellationToken);
        }
    }
}
