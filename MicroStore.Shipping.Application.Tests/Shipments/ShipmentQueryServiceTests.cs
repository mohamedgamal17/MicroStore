using FluentAssertions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Shipping.Application.Shipments;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Shipping.Application.Tests.Shipments
{
    public class ShipmentQueryServiceTests : BaseTestFixture
    {
        private readonly IShipmentQueryService _shipmentQueryService;

        public ShipmentQueryServiceTests()
        {
            _shipmentQueryService = GetRequiredService<IShipmentQueryService>();
        }
        [Test]
        public async Task Should_get_shipment_list_paged()
        {
            var query = new PagingQueryParams();


            var result = await _shipmentQueryService.ListAsync(query);

            result.IsSuccess.Should().BeTrue();


            result.Value.Skip.Should().Be(query.Skip);
            result.Value.Lenght.Should().Be(query.Lenght);
            result.Value.Items.Count().Should().BeLessThanOrEqualTo(query.Lenght);
        }

        [Test]
        public async Task Should_get_user_shipment_list_paged()
        {
            var query = new PagingQueryParams();


            string userId = "bfdd1deb-167d-4269-b0e3-351613b8a202";

            var result = await _shipmentQueryService.ListAsync(query,userId);


            result.IsSuccess.Should().BeTrue();

            result.Value.Skip.Should().Be(query.Skip);
            result.Value.Lenght.Should().Be(query.Lenght);
            result.Value.Items.Count().Should().BeLessThanOrEqualTo(query.Lenght);
            result.Value.Items.All(x => x.UserId == userId).Should().BeTrue();


        }


        [Test]
        public async Task Should_get_shipment_with_id()
        {
            string shipmentId = (await First<Shipment>()).Id;


            var result = await _shipmentQueryService.GetAsync(shipmentId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(shipmentId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_shipment_by_id_when_shipment_is_not_exist()
        {
            var result = await _shipmentQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>(); ;
        }


        [Test]
        public async Task Should_get_shipment_with_order_id()
        {
            string orderId = "379a9bf2-c85f-49c3-8422-6d6a10999bd6";


            var result = await _shipmentQueryService.GetByOrderIdAsync(orderId);

            result.IsSuccess.Should().BeTrue();

            result.Value.OrderId.Should().Be(orderId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_shipment_by_order_id_when_shipment_is_not_exist()
        {
            var result = await _shipmentQueryService.GetByOrderIdAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>(); ;
        }


        [Test]
        public async Task Should_get_shipment_with_order_number()
        {
            string orderNumber = "023b4122-26a9-43cb-81b6-433e2f3d292e";


            var result = await _shipmentQueryService.GetByOrderNumberAsync(orderNumber);

            result.IsSuccess.Should().BeTrue();

            result.Value.OrderNumber.Should().Be(orderNumber);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_shipment_by_order_number_when_shipment_is_not_exist()
        {
            var result = await _shipmentQueryService.GetByOrderNumberAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>(); ;
        }
    }
}
