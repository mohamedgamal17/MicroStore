using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Interfaces;

using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiResources
{
    public abstract class ApiResourceSecretCommandTestBase : BaseTestFixture
    {
        public const string SecretType = "SharedSecret";

        public ICryptoServiceProvider CryptoServiceProvider { get; set; }

        public ApiResourceSecretCommandTestBase()
        {
            CryptoServiceProvider = ServiceProvider.GetRequiredService<ICryptoServiceProvider>();
        }

        public Task<ApiResource> CreateFakeApiResource()
        {
            var apiResource = new ApiResource
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
            };

            return Insert(apiResource);
        }



        public async Task<ApiResource> CreateFakeApiResourceWithSecret() 
        {
            var apiResource = await CreateFakeApiResource();

            apiResource.Secrets = new List<Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret>
            {
                new Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret
                {
                    Value = (await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32)).ToSha512(),
                    Type = SecretType,
                     Description = Guid.NewGuid().ToString()
                }
            };

            return await Update(apiResource);
        }
    }

    public class When_receving_add_api_resource_secret_command : ApiResourceSecretCommandTestBase
    {

        [Test]
        public async Task Should_create_api_resource_secret()
        {

            var fakeApiResource = await CreateFakeApiResource();

            var command = new AddApiResourceSecretCommand
            {
                Algoritm = Common.Models.HasingAlgoritm.SHA512,
                Description = Guid.NewGuid().ToString(),
                Value = await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32),
                ApiResourceId = fakeApiResource.Id
            };

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var apiResource = await SingleAsync<ApiResource>(x => x.Id == response.EnvelopeResult.Result.Id);

            apiResource.Secrets.Count.Should().Be(1);

            var apiSecret = apiResource.Secrets.First();

            apiSecret.Type.Should().Be(SecretType);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_while_api_resource_is_not_exist()
        {
            var command = new AddApiResourceSecretCommand
            {
                Algoritm = Common.Models.HasingAlgoritm.SHA512,
                Description = Guid.NewGuid().ToString(),
                Value = await CryptoServiceProvider.GenerateRandomEncodedBase64Key(32),
                ApiResourceId = int.MaxValue
            };



            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

    }


    public class When_receving_remove_api_resource_secret_command : ApiResourceSecretCommandTestBase
    {

        [Test]
        public async Task Should_remove_api_resource_secret()
        {
            var fakeApiResource = await CreateFakeApiResourceWithSecret();

            var command = new RemoveApiResourceSecretCommand
            {
                ApiResourceId = fakeApiResource.Id,
                SecretId = fakeApiResource.Secrets.First().Id
            };

            var response = await Send(command);
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);


            var apiResource = await SingleAsync<ApiResource>(x => x.Id == fakeApiResource.Id);

            apiResource.Secrets.Count.Should().Be(0);

        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_while_api_resource_is_not_exist()
        {
            var command = new RemoveApiResourceSecretCommand
            {
                ApiResourceId = int.MaxValue,
               
            };

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_400_badrequest_while_api_resource_not_contain_specific_secret()
        {
            var fakeApiResource = await CreateFakeApiResourceWithSecret();

            var command = new RemoveApiResourceSecretCommand
            {
                ApiResourceId = fakeApiResource.Id,
                SecretId = int.MaxValue

            };

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


    }
}
