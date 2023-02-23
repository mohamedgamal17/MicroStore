﻿using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Clients
{
    public class ClientQueryServiceTests : BaseTestFixture
    {

        private readonly IClientQueryService _clientQueryService;
        public ClientQueryServiceTests()
        {
            _clientQueryService=  GetRequiredService<IClientQueryService>();    
        }

        [Test]
        public async Task Should_return_paged_list_client()
        {
            var queryParams = new PagingQueryParams() { PageSize = 3 };

            var result = await _clientQueryService.ListAsync(queryParams);

            result.IsSuccess.Should().BeTrue();

            result.Result.PageSize.Should().Be(queryParams.PageSize);

            result.Result.Items.Count().Should().BeLessThanOrEqualTo(queryParams.PageSize);
        }


        [Test]
        public async Task Should_get_client_by_id()
        {
            var client = await FirstAsync<Client>();

            var result = await _clientQueryService.GetAsync(client.Id);

            result.IsSuccess.Should().BeTrue();

            result.Result.Id.Should().Be(client.Id);

        }


        [Test]
        public async Task Should_return_failure_result_while_getting_client_by_id_when_client_is_not_exist()
        {
            var result = await _clientQueryService.GetAsync(int.MaxValue);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }
    }

 
}