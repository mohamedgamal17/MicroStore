using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;

using MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions;
using System.Net;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.ApiScopes
{
    public class When_receving_create_api_scope_command : BaseTestFixture
    {
        [Test]
        public async Task Should_create_api_scope()
        {
            var command = GenerateApiScopeCommand();

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var apiScopeId = response.EnvelopeResult.Result.Id;

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == apiScopeId);

            apiScope.AssertApiScope(response.EnvelopeResult.Result);

            apiScope.AssertApiScopeCommand(command);
        }

        private CreateApiScopeCommand GenerateApiScopeCommand()
        {
            return new CreateApiScopeCommand
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                ShowInDiscoveryDocument = true,
                Emphasize = true,
                UserClaims = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },
                Properties = new List<PropertyModel>
                {
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()}
                },

            };
        }

    }

    public class When_receving_update_api_scope : BaseTestFixture
    {
        [Test]
        public async Task Should_update_api_scope()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var command =  GenerateApiScopeCommand();

            command.ApiScopeId = fakeApiScope.Id;

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int) HttpStatusCode.OK);

            var apiScope = await SingleAsync<ApiScope>(x => x.Id == command.ApiScopeId);

            apiScope.AssertApiScope(response.EnvelopeResult.Result);

            apiScope.AssertApiScopeCommand(command);
        }


        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_scope_is_not_exist()
        {
            var command = GenerateApiScopeCommand();

            command.ApiScopeId = int.MaxValue;

            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private Task<ApiScope> GenerateFakeApiScope()
        {
            var apiScope = new ApiScope
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
            };

            return Insert(apiScope);
        }
        private UpdateApiScopeCommand GenerateApiScopeCommand()
        {
            return new UpdateApiScopeCommand
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
                ShowInDiscoveryDocument = true,
                Emphasize = true,
                UserClaims = new List<string>
                {
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                },
                Properties = new List<PropertyModel>
                {
                    new PropertyModel{Key = Guid.NewGuid().ToString() , Value= Guid.NewGuid().ToString()}
                },

            };
        }
    }


    public class When_receving_remove_api_scope_command : BaseTestFixture
    {
        [Test]
        public async Task Should_remove_api_scope()
        {
            var fakeApiScope = await GenerateFakeApiScope();

            var command = new RemoveApiScopeCommand { ApiScopeId = fakeApiScope.Id };

            var response = await Send(command);

            response.IsSuccess.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            var apiScope = await SingleOrDefaultAsync<ApiScope>(x => x.Id == fakeApiScope.Id);

            apiScope.Should().BeNull();
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_notfound_while_api_scope_is_not_exist()
        {
            var command = new RemoveApiScopeCommand { ApiScopeId = int.MaxValue };


            var response = await Send(command);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        private Task<ApiScope> GenerateFakeApiScope()
        {
            var apiScope = new ApiScope
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                DisplayName = Guid.NewGuid().ToString(),
            };

            return Insert(apiScope);
        }
    }

}
