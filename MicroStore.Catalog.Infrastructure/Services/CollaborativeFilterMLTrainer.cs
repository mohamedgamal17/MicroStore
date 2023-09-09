using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Domain.Configuration;
using MicroStore.Catalog.Domain.ML;
using Microsoft.ML.Trainers;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Catalog.Infrastructure.Services
{
    internal class CollaborativeFilterMLTrainer : ICollaborativeFilterMLTrainer , ITransientDependency
    {
        private readonly ElasticsearchClient _elasticSearchClient;

        private readonly ICatalogDbContext _catalogDbContet;

        private readonly ApplicationSettings _applicationSettings;

        private readonly MLContext _mlContext;

        public CollaborativeFilterMLTrainer(ElasticsearchClient elasticSearchClient, ICatalogDbContext catalogDbContet, ApplicationSettings applicationSettings)
        {

            _elasticSearchClient = elasticSearchClient;
            _catalogDbContet = catalogDbContet;
            _mlContext = new MLContext();
            _applicationSettings = applicationSettings;
        }

        public async Task ReindexAsync()
        {
            await RemoveExpectedRatingDocs();

            var model = await TrainAsync();

            var predicationEngine = _mlContext.Model.CreatePredictionEngine<ProductRating, ProductRatingPrediction>(model);

            int length = 0;

            int skip = 0;

            do
            {
                var pagedResult = await GetUserUnRatedProducts();

                length = pagedResult.Lenght;

                skip = pagedResult.Skip;

                await IndexPredications(predicationEngine ,pagedResult.Items);

            } while (length > skip);
        }

        private async Task<ITransformer> TrainAsync()
        {
            var dataView = await PreapreTrainData();

            var pipeline = BuildTrainPipeline();

            var model = pipeline.Fit(dataView);

            return model;
        }


        private async Task<IDataView> PreapreTrainData()
        {

            var query = from pr in _catalogDbContet.ProductReviews
                        select new ProductRating
                        {
                            UserId = pr.UserId,
                            ProductId = pr.ProductId,
                            Rating = pr.Rating
                        };
            var data = await query.ToListAsync();

            return _mlContext.Data.LoadFromEnumerable<ProductRating>(data);
        }

        private IEstimator<ITransformer> BuildTrainPipeline()
        {
            var matrixFactorizationOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserId",
                MatrixRowIndexColumnName = "ProductId",
                LabelColumnName = "Label",
                NumberOfIterations = 150,
                ApproximationRank = 20
            };

            var pipline = _mlContext.Transforms.Categorical.OneHotEncoding("UserId")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("ProductId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(matrixFactorizationOptions));

            return pipline;

        }

        private async Task RemoveExpectedRatingDocs()
        {
            await _elasticSearchClient.DeleteByQueryAsync<ElasticProductExpectedRating>(ElasticEntitiesConsts.ProductIndex, desc => desc.Query(qr => qr.MatchAll())
            );
        }

        private async Task<PagedResult<UserUnRatedProduct>> GetUserUnRatedProducts(int skip = 0, int size = 15)
        {

            var query = from userId in _catalogDbContet.ProductReviews.Select(x => x.UserId).Distinct()
                        from review in _catalogDbContet.ProductReviews
                        where userId != review.UserId
                        select new UserUnRatedProduct
                        {
                            UserId = userId,
                            ProductId = review.ProductId
                        };

            return await query.PageResult(skip, size);
        }

        private async Task IndexPredications(PredictionEngine<ProductRating, ProductRatingPrediction> predicationEngine, IEnumerable<UserUnRatedProduct> data)
        {
            foreach (var item in data)
            {
                var predication = predicationEngine.Predict(new ProductRating
                {
                    UserId = item.UserId,
                    ProductId = item.ProductId
                });

                if (Math.Round(predication.Score, 1) > 2.5)
                {
                    var reccomandedProduct = new ElasticProductExpectedRating
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = item.ProductId,
                        UserId = item.UserId,
                        Score = predication.Score
                    };

                    await _elasticSearchClient.IndexAsync(reccomandedProduct);
                }
            }
        }



        private class UserUnRatedProduct
        {
            public string UserId { get; set; }

            public string ProductId { get; set; }
        }
    }
}
