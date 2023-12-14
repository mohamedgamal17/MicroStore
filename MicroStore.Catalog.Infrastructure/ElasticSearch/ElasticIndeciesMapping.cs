using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Infrastructure.ElasticSearch
{
    public class ElasticIndeciesMapping
    {
        public static CreateIndexRequestDescriptor<ElasticImageVector> ImageVectorMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticImageVector>(IndexName.From<ElasticImageVector>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .DenseVector(x => x.Features, cf => cf.Dims(1440).Similarity("l2_norm").Index(true))
                        .Keyword(x=> x.ProductId)
                        .Keyword(x=> x.ImageId)
                    )
                );
        }
        public static CreateIndexRequestDescriptor<ElasticCategory> ElasticCategoryMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticCategory>(IndexName.From<ElasticCategory>())
                .Mappings(mp => mp
                    .Properties(pr=> pr
                        .Text(x=> x.Name)
                        .Text(x=> x.Description)
                         .Boolean(x => x.IsDeleted)
                        .Date(x => x.CreatationTime)
                        .Date(x => x.LastModificationTime)
                        .Date(x => x.DeletionTime)
                        .Text(x => x.CreatorId)
                        .Keyword(x => x.LastModifierId)
                        .Keyword(x => x.DeleterId)
                    )
                );

        }

        public static CreateIndexRequestDescriptor<ElasticManufacturer> ElasticManufacturerMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticManufacturer>(IndexName.From<ElasticManufacturer>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Text(x => x.Name)
                        .Text(x => x.Description)
                        .Boolean(x => x.IsDeleted)
                        .Date(x => x.CreatationTime)
                        .Date(x => x.LastModificationTime)
                        .Date(x => x.DeletionTime)
                        .Text(x => x.CreatorId)
                        .Keyword(x => x.LastModifierId)
                        .Keyword(x => x.DeleterId)
                    )
                );
        }


        public static CreateIndexRequestDescriptor<ElasticProductTag> ElasticProductTagMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticProductTag>(IndexName.From<ElasticProductTag>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Text(x => x.Name)
                        .Text(x => x.Description)
                    )
                );
        }


        public static CreateIndexRequestDescriptor<ElasticSpecificationAttribute> ElasticSpecificationAttributeMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticSpecificationAttribute>(
                IndexName.From<ElasticSpecificationAttribute>())
                .Mappings(mp => mp
                    .Properties(pr=> pr
                        .Text(x=> x.Name)
                        .Text(x=> x.Description)
                        .Nested(x=>  x.Options,cfg=> cfg
                            .Properties(prx=> prx
                                .Keyword(x=> x.Options.First().Id)
                                .Keyword(x=> x.Options.First().Value)
                            )
                        )
                    )
                );
        }

        public static CreateIndexRequestDescriptor<ElasticProduct> ElasticProductMappings(string indexName)
        {

            return new CreateIndexRequestDescriptor<ElasticProduct>(IndexName.From<ElasticProduct>())
                .Mappings(mp =>
                    mp.Properties(pr=> pr
                        .Text(x=> x.Name)
                        .Text(x=> x.ShortDescription)
                        .Text(x=> x.LongDescription)
                        .FloatNumber(x=> x.Price)
                        .FloatNumber(x=> x.OldPrice)
                        .Boolean(x=> x.IsFeatured)
                        .Object(x=> x.Weight, cfg=> cfg
                            .Properties(prx=> prx
                                .FloatNumber(c=> c.Weight.Value)
                                .Keyword(c=>c.Weight.Unit)
                            )
                         )
                        .Object(x => x.Dimensions, cfg => cfg
                            .Properties(prx => prx
                                .FloatNumber(c => c.Dimensions.Length)
                                .FloatNumber(c => c.Dimensions.Width)
                                .FloatNumber(c => c.Dimensions.Height)
                                .Keyword(c => c.Weight.Unit)
                            )
                         )
                        .Nested(x=> x.ProductCategories , cfg=> cfg
                            .Properties(prx=> prx
                                .Keyword(x=> x.ProductCategories.First().Id)
                                .Keyword(x=> x.ProductCategories.First().CategoryId)
                                .Keyword(x=> x.ProductCategories.First().Name)
                            )
                        )
                        .Nested(x => x.ProductManufacturers, cfg => cfg
                            .Properties(prx => prx
                                .Keyword(x => x.ProductManufacturers.First().Id)
                                .Keyword(x => x.ProductManufacturers.First().ManufacturerId)
                                .Keyword(x => x.ProductManufacturers.First().Name)
                            )
                        )
                        .Nested(x => x.ProductTags, cfg => cfg
                            .Properties(prx => prx
                                .Keyword(x => x.ProductTags.First().Id)
                                .Keyword(x => x.ProductTags.First().Name)
                                .Text(x => x.ProductTags.First().Description)
                            )
                        )
                        .Nested(x => x.ProductImages, cfg => cfg
                            .Properties(prx => prx
                                .Keyword(x => x.ProductImages.First().Id)
                                .Text(x => x.ProductImages.First().Image)
                                .IntegerNumber(x => x.ProductImages.First().DisplayOrder)
                            )
                        )
                         .Nested(x => x.SpecificationAttributes, cfg => cfg
                            .Properties(prx => prx
                                .Keyword(x => x.SpecificationAttributes.First().Id)
                                .Keyword(x => x.SpecificationAttributes.First().AttributeId)
                                .Keyword(x => x.SpecificationAttributes.First().OptionId)
                                .Keyword(x => x.SpecificationAttributes.First().Name)
                                .Keyword(x => x.SpecificationAttributes.First().Value)
                            )
                        )
                        .Boolean(x => x.IsDeleted)
                        .Date(x => x.CreatationTime)
                        .Date(x => x.LastModificationTime)
                        .Date(x => x.DeletionTime)
                        .Text(x => x.CreatorId)
                        .Keyword(x => x.LastModifierId)
                        .Keyword(x => x.DeleterId)


                    )
                );
        }


        public static CreateIndexRequestDescriptor<ElasticProductReview> ElasticProductReviewMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticProductReview>(IndexName.From<ElasticProductReview>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Keyword(x => x.UserId)
                        .Keyword(x => x.ProductId)
                        .Text(x => x.Title)
                        .Text(x => x.ReviewText)
                        .IntegerNumber(x => x.Rating)
                        .Text(x => x.ReplayText)
                        .Boolean(x=> x.IsDeleted)
                        .Date(x => x.CreatationTime)
                        .Date(x => x.LastModificationTime)
                        .Date(x => x.DeletionTime)
                        .Text(x => x.CreatorId)
                        .Keyword(x => x.LastModifierId)
                        .Keyword(x => x.DeleterId)
                    )
                );
        }

        public static CreateIndexRequestDescriptor<ElasticProductExpectedRating> ElasticProductExpectedRatingMappings(string indexName)
        {
            return new CreateIndexRequestDescriptor<ElasticProductExpectedRating>(IndexName.From<ElasticProductExpectedRating>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Keyword(x => x.ProductId)
                        .Keyword(x => x.UserId)
                        .FloatNumber(x => x.Score)
                    )
                 );
        }


    }
}
