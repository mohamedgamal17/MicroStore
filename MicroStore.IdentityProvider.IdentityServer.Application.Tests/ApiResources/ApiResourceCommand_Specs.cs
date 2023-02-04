using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{
    public  class When_reciving_create_api_resource_command : BaseTestFixture
    {
        [Test]
        public async Task Should_create_api_resource()
        {

            var command  =GenerateApiResourceCommand();

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var apiResourceId = response.EnvelopeResult.Result.Id;

            var apiResource = await SingleAsync<ApiResource>(x=> x.Id == apiResourceId);

            apiResource.AssertApiResource(response.EnvelopeResult.Result);

            apiResource.AssertApiResourceCommand(command);
        }


        private CreateApiResourceCommand GenerateApiResourceCommand()
        {
            return new CreateApiResourceCommand
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
    }

    public class When_receving_update_api_resource_command : BaseTestFixture
    {
        [Test]
        public async Task Should_update_api_resource()
        {
            var fakeApiReesource = await CreateFakeApiResource();

            var command = GenerateApiResourceCommand();

            command.ApiResourceId = fakeApiReesource.Id;

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var apiResourceId = response.EnvelopeResult.Result.Id;

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == apiResourceId);

            apiResource.AssertApiResource(response.EnvelopeResult.Result);

            apiResource.AssertApiResourceCommand(command);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_resource_is_not_exist()
        {
            var command = GenerateApiResourceCommand();

            command.ApiResourceId = int.MaxValue;

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        private UpdateApiResourceCommand GenerateApiResourceCommand()
        {
            return new UpdateApiResourceCommand
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

        private Task<ApiResource> CreateFakeApiResource()
        {
            var apiResource = new ApiResource
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
            };

            return Insert(apiResource);
        }
    }



    public class When_receving_remove_api_resource_command : BaseTestFixture
    {
        [Test]
        public async Task Should_remove_api_resource()
        {
            var fakeApiReesource = await CreateFakeApiResource();

            var command = new RemoveApiResourceCommand { ApiResourceId = fakeApiReesource.Id };

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            var apiResponse = await SingleOrDefaultAsync<ApiResource>(x => x.Id == command.ApiResourceId);

            apiResponse.Should().BeNull();

        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_resource_is_not_exist()
        {
            var command = new RemoveApiResourceCommand { ApiResourceId = int.MaxValue };

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private Task<ApiResource> CreateFakeApiResource()
        {
            var apiResource = new ApiResource
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
            };

            return Insert(apiResource);
        }
    }


}
