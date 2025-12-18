using Newtonsoft.Json;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities
{
    public class CreationAuditedEntity<TKey> : BaseEntity<TKey>
    {
        [JsonProperty("creation_time")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("creator_id")]
        public string CreatorId { get; set; }

    }

    public class ModificationAuditedEntity<TKey> : CreationAuditedEntity<TKey>
    {
        [JsonProperty("last_modification_time")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("last_modifier_id")]
        public string ModifierId { get; set; }

    }

    public class AuditedEntity<TKey> : ModificationAuditedEntity<TKey> { }
}
