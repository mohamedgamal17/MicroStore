using FluentAssertions;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Domain.Shared;

namespace MicroStore.Payment.Application.Tests.Services
{
    [TestFixture]
    public class PaymentRequestService_Specs : BaseTestFixture
    {

        private readonly IPaymentRequestService _sut;

        public PaymentRequestService_Specs()
        {
            _sut = GetRequiredService<IPaymentRequestService>();
        }


        [Test]
        public async Task Should_complete_payment()
        {
            PaymentRequest fakePaymentRequest = await CreateFakeOpenedPaymentRequest();

            var result = await _sut.CompletePayment(FakePaymentData.TransactionId, FakePaymentData.PaymentGateway, FakePaymentData.CapturedAt);

            result.IsSuccess.Should().BeTrue();

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakePaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Completed);
        }

        [Test]
        public async Task Should_return_faild_result_while_completing_payment_request_when_payment_request_is_not_exist()
        {
            var result = await _sut.CompletePayment(FakePaymentData.TransactionId, FakePaymentData.PaymentGateway, FakePaymentData.CapturedAt);

            result.IsFailure.Should().BeTrue();

        }

        [Test]
        public async Task Should_return_faild_result_while_completing_payment_request_when_payment_request_is_in_inconsisten_state()
        {
            PaymentRequest fakePaymentRequest = await CreateFakePaymentRequest();

            var result = await _sut.CompletePayment(FakePaymentData.TransactionId, FakePaymentData.PaymentGateway, FakePaymentData.CapturedAt);

            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public async Task Should_get_payment_request_successfully()
        {
            PaymentRequest fakePaymentRequest = await CreateFakePaymentRequest();

            var result = await _sut.GetPayment(fakePaymentRequest.Id);

            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_faild_result_while_getting_payment_request_when_payment_request_is_not_exist()
        {
            var result = await _sut.GetPayment(Guid.NewGuid());

            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public async Task Should_set_payment_request_faild()
        {
            var fakePaymentRequest = await CreateFakeOpenedPaymentRequest();

            var result = await _sut.SetPaymentFaild(fakePaymentRequest.Id, FakePaymentData.FaultReason, DateTime.UtcNow);

            result.IsSuccess.Should().BeTrue();

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakePaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Faild);

            paymentRequest.FaultReason.Should().Be(FakePaymentData.FaultReason);
        }

        [Test]
        public async Task Should_return_faild_result_while_try_to_faild_payment_request_when_payment_request_is_not_exist()
        {
            var result = await _sut.SetPaymentFaild(Guid.NewGuid(), FakePaymentData.FaultReason, DateTime.UtcNow);

            result.IsFailure.Should().BeTrue();

        }

        [Test]
        public async Task Should_return_faild_result_while_try_to_faild_payment_request_when_payment_request_is_in_inconsistent_state()
        {
            var fakePayment = await CreateFakePaymentRequest();

            var result = await _sut.SetPaymentFaild(fakePayment.Id, FakePaymentData.FaultReason, DateTime.UtcNow);

            result.IsFailure.Should().BeTrue();

        }

        [Test]
        public async Task Should_start_payment_request_successfully()
        {
            var fakePayment = await CreateFakePaymentRequest();

            var result = await _sut.StartPayment(fakePayment.Id, FakePaymentData.TransactionId, FakePaymentData.PaymentGateway, FakePaymentData.OpenedAt);

            result.IsSuccess.Should().BeTrue();

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakePayment.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Opened);

            paymentRequest.TransctionId.Should().Be(FakePaymentData.TransactionId);

            paymentRequest.PaymentGateway.Should().Be(FakePaymentData.PaymentGateway);

        }

        [Test]
        public async Task Should_return_faild_result_when_starting_payment_request_while_payment_request_is_not_exist()
        {
            var result = await  _sut.StartPayment(Guid.NewGuid(), FakePaymentData.TransactionId, FakePaymentData.TransactionId, FakePaymentData.OpenedAt);

            result.IsFailure.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_faild_result_when_starting_payment_request_while_payment_request_is_in_inconsistent_state()
        {
            var fakePaymentRequest = await CreateFakeVoidedPaymentRequest();

            var result  = await _sut.StartPayment(fakePaymentRequest.Id, FakePaymentData.TransactionId, FakePaymentData.TransactionId, FakePaymentData.OpenedAt);

            result.IsFailure.Should().BeTrue();
        }

        private Task<PaymentRequest> CreateFakePaymentRequest()
        {
            return Insert(new PaymentRequest(FakePaymentData.OrderId, FakePaymentData.OrderNumber, FakePaymentData.CustomerId, FakePaymentData.Amount));

        }

        private Task<PaymentRequest> CreateFakeOpenedPaymentRequest()
        {
            PaymentRequest paymentRequest = new PaymentRequest(FakePaymentData.OrderId, FakePaymentData.OrderNumber, FakePaymentData.CustomerId, FakePaymentData.Amount);

            paymentRequest.SetPaymentOpened(FakePaymentData.TransactionId, FakePaymentData.OpenedAt, FakePaymentData.PaymentGateway);

            return Insert(paymentRequest);
        }

        private Task<PaymentRequest> CreateFakeVoidedPaymentRequest()
        {
            PaymentRequest paymentRequest = new PaymentRequest(FakePaymentData.OrderId, FakePaymentData.OrderNumber, FakePaymentData.CustomerId, FakePaymentData.Amount);

            paymentRequest.VoidPayment(FakePaymentData.FaultDate);

            return Insert(paymentRequest);
        }


        private static class FakePaymentData
        {
            public readonly static Guid OrderId = Guid.NewGuid();

            public readonly static string OrderNumber = Guid.NewGuid().ToString();

            public readonly static string CustomerId = Guid.NewGuid().ToString();

            public readonly static string TransactionId = Guid.NewGuid().ToString();

            public readonly static decimal Amount = 50;

            public readonly static string PaymentGateway = Guid.NewGuid().ToString();

            public readonly static string FaultReason = Guid.NewGuid().ToString();

            public readonly static DateTime OpenedAt = DateTime.UtcNow;

            public readonly static DateTime CapturedAt = DateTime.UtcNow;

            public readonly static DateTime FaultDate = DateTime.UtcNow;


        }
    }
}
