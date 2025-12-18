using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.TestBase;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Catalog.Application.Tests
{
    [TestFixture]
    public abstract class BaseTestFixture : MassTransitTestBase<StartupModule>
    {
        protected ElasticsearchClient  ElasticsearchClient { get; set; }

        protected IObjectMapper ObjectMapper { get; set; }
        public BaseTestFixture()
        {
            ElasticsearchClient = ServiceProvider.GetRequiredService<ElasticsearchClient>();

            ObjectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [OneTimeSetUp]
        protected async Task SetupBeforeAllTests()
        {         
            await StartMassTransit();
        }

        [OneTimeTearDown]
        protected async Task SetupAfterRunAnyTest()
        {
            await StopMassTransit();
        }


        public async Task<T?> FindElasticDoc<T>(string id) 
        {
            var response = await ElasticsearchClient.GetAsync<T>(id);

            if (response.IsValidResponse)
            {
                return response.Source;
            }

            return default(T);
        }

        public async Task CreateElasticDoc<T> (T doc)
        {
            await ElasticsearchClient.IndexAsync(doc);         
        }

    }
}
