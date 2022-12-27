#nullable disable
using MicroStore;
namespace MicroStore.Payment.Application.Abstractions.Models
{
    public class ProcessPaymentModel
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }

    }
}
