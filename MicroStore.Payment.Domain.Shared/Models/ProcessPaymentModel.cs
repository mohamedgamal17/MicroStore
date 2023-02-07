#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class ProcessPaymentModel
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }

    }
}
