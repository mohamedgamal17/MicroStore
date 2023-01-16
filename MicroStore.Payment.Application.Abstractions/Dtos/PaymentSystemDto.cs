using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Dtos
{
    public class PaymentSystemDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
