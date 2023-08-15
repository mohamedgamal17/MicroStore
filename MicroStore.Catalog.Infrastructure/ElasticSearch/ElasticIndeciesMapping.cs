using Elastic.Clients.Elasticsearch.IndexManagement;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Infrastructure.ElasticSearch
{
    public class ElasticIndeciesMapping
    {


        public static CreateIndexRequestDescriptor<ImageVector> ImageVectorMappings()
        {
            return new CreateIndexRequestDescriptor<ImageVector>(ImageVector.INDEX_NAME)
                .Mappings(mp => mp
                    .Properties(pr => pr
                        .DenseVector(x => x.Features, cf => cf.Index(true).Similarity("l2_norm"))
                        .Text(x=> x.ProductId)
                        .Text(x=> x.ImageId)
                    )
                );
        }
    }
}
