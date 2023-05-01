using Duende.IdentityServer.EntityFramework.Entities;
using IdentityModel;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{
    public class ApiResourceCommandServiceBaseTest : BaseTestFixture
    {
        public const string SecretType = "SharedSecret";
        protected ApiResourceModel PrepareApiResourceModel()
        {
            return new ApiResourceModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                Scopes = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },

                Properties = new List<PropertyModel>()
                {
                    new PropertyModel{ Key= Guid.NewGuid().ToString(), Value= Guid.NewGuid().ToString() },
                    new PropertyModel{ Key= Guid.NewGuid().ToString(), Value= Guid.NewGuid().ToString() },
                    new PropertyModel{ Key= Guid.NewGuid().ToString(), Value= Guid.NewGuid().ToString() }
                },

                UserClaims = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },

            };
        }

        protected SecretModel PrepareSecretModel()
        {
            return new SecretModel
            {
                Value = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };
        }

        protected Task<ApiResource> CreateFakeApiResource()
        {
            var apiResource = new ApiResource
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                Properties = new List<ApiResourceProperty>
                {
                    new ApiResourceProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiResourceProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiResourceProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                    new ApiResourceProperty{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()},
                }
            };

            return Insert(apiResource);
        }

        protected async Task<ApiResource> CreateFakeApiResourceWithSecret()
        {
            var apiResource = await CreateFakeApiResource();

            apiResource.Secrets = new List<Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret>
            {
                new Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret
                {
                    Value = Guid.NewGuid().ToString().ToSha512(),
                    Type = SecretType,
                    Description = Guid.NewGuid().ToString()
                },
            };

            return await Update(apiResource);
        }
    }

}
