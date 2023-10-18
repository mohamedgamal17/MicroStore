using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Infrastructure.ElasticSearch
{
    public class ElasticIndeciesMapping
    {
        public static CreateIndexRequestDescriptor<ElasticImageVector> ImageVectorMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticImageVector>(ElasticEntitiesConsts.ImageVectorIndex)
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .DenseVector(x => x.Features, cf => cf.Index(true).Similarity("l2_norm"))
                        .Text(x=> x.ProductId)
                        .Text(x=> x.ImageId)
                    )
                );
        }
        public static CreateIndexRequestDescriptor<ElasticCategory> ElasticCategoryMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticCategory>(IndexName.From<ElasticCategory>())
                .Mappings(mp => mp
                    .Properties(pr=> pr
                        .Keyword(x=> x.Name)
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

        public static CreateIndexRequestDescriptor<ElasticManufacturer> ElasticManufacturerMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticManufacturer>(IndexName.From<ElasticManufacturer>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Keyword(x => x.Name)
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


        public static CreateIndexRequestDescriptor<ElasticProductTag> ElasticProductTagMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticProductTag>(IndexName.From<ElasticProductTag>())
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .Keyword(x => x.Name)
                        .Text(x => x.Description)
                    )
                );
        }


        public static CreateIndexRequestDescriptor<ElasticSpecificationAttribute> ElasticSpecificationAttributeMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticSpecificationAttribute>(
                IndexName.From<ElasticSpecificationAttribute>())
                .Mappings(mp => mp
                    .Properties(pr=> pr
                        .Keyword(x=> x.Name)
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

        public static CreateIndexRequestDescriptor<ElasticProduct> ElasticProductMappings()
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
                                .Keyword(x=> x.ProductCategories.First().Name)
                                .Text(x=> x.ProductCategories.First().Description)
                            )
                        )
                        .Nested(x => x.ProductManufacturers, cfg => cfg
                            .Properties(prx => prx
                                .Keyword(x => x.ProductManufacturers.First().Id)
                                .Keyword(x => x.ProductManufacturers.First().Name)
                                .Text(x => x.ProductManufacturers.First().Description)
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


        public static CreateIndexRequestDescriptor<ElasticProductReview> ElasticProductReviewMappings()
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

        public static CreateIndexRequestDescriptor<ElasticProductExpectedRating> ElasticProductExpectedRatingMappings()
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
