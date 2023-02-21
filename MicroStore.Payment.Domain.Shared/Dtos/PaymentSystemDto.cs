using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Domain.Shared.Dtos
{
    public class PaymentSystemDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
