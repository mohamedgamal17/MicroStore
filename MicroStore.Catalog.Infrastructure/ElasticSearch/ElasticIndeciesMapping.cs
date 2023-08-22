using Elastic.Clients.Elasticsearch.IndexManagement;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Infrastructure.ElasticSearch
{
    public class ElasticIndeciesMapping
    {


        public static CreateIndexRequestDescriptor<ElasticImageVector> ImageVectorMappings()
        {
            return new CreateIndexRequestDescriptor<ElasticImageVector>(ImageVector.INDEX_NAME)
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
