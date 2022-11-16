using FluentAssertions;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.Tests.Domain
{
    public class PaymentRequest_Tests
    {


        [Test]
        public void Should_complete_payment_reqeust_successfully()
        {

            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            paymentRequest.SetPaymentOpened("faketransaction", DateTime.UtcNow, "fakegateway");

            paymentRequest.SetPaymentCompleted(DateTime.UtcNow);

            paymentRequest.State.Should().Be(PaymentStatus.Completed);

        }

        [Test]
        public void Should_void_payment_request_successfully()
        {
            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            paymentRequest.VoidPayment(DateTime.UtcNow);

            paymentRequest.State.Should().Be(PaymentStatus.Void);
        }

        [Test]
        public void Should_payment_request_fault()
        {
            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            paymentRequest.SetPaymentOpened("faketransaction", DateTime.UtcNow, "fakegateway");

            paymentRequest.SetPaymentFaild("fakereason", DateTime.UtcNow);

            paymentRequest.State.Should().Be(PaymentStatus.Faild);
        }

        [Test]
        public void Should_throw_invalid_operation_exception_while_completing_payment_when_payment_state_is_not_opened()
        {
            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            Action action = () => paymentRequest.SetPaymentCompleted(DateTime.UtcNow);

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_invalid_operation_exception_while_opening_payment_when_payment_state_is_not_created()
        {
            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            paymentRequest.VoidPayment(DateTime.UtcNow);

            Action action = () => paymentRequest.SetPaymentOpened("faketransaction", DateTime.UtcNow, "fakegateway");

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void Should_throw_invalid_operation_exception_while_payment_fault_when_payment_state_is_not_opened()
        {
            PaymentRequest paymentRequest = new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50);

            Action action = () =>  paymentRequest.SetPaymentFaild("fakereason", DateTime.UtcNow);

            action.Should().ThrowExactly<InvalidOperationException>();
        }
    }
}
