
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Api.Dtos;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Api.Services;
using MicroStore.TestBase;
using MicroStore.TestBase.Extensions;
using Moq;

namespace MicroStore.Payment.Api.Tests
{
    public class MassTransitTestFixture : MassTransitTestBase<PaymentApiTestModule>
    {

        public Mock<IPaymentService> MockedPaymentService { get; private set; }

        public MassTransitTestFixture()
        {
            MockedPaymentService = new Mock<IPaymentService>();

            MockedPaymentService.Setup(c => c.CreatePayment(It.IsAny<CreatePaymentModel>()))
                .ReturnsAsync(new PaymentDto
                {
                    Amount = 5000,
                    TransactionId = Guid.NewGuid().ToString()
                });

            MockedPaymentService.Setup(c => c.CapturePayment(It.IsAny<string>()));

            MockedPaymentService.Setup(c => c.RefundPayment(It.IsAny<string>()));

        }

        [OneTimeSetUp]
        public Task SetupBeforeAnyTest()
        {
            return StartMassTransit();
        }


        [OneTimeTearDown]
        public Task SetupAfterAllTests()
        {
            return StartMassTransit();
        }


        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.Remove<IPaymentService>();

            services.AddTransient(typeof(IPaymentService), _ => MockedPaymentService.Object);
        }
    }
}
