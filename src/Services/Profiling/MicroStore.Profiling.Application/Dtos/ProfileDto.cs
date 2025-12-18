using Volo.Abp.Application.Dtos;
namespace MicroStore.Profiling.Application.Dtos
{
    public class ProfileDto : FullAuditedEntityDto<string>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string? Avatar { get; set; }
        public List<AddressDto>? Addresses { get; set; }
    }
}
