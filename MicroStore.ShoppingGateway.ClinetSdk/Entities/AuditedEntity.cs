namespace MicroStore.ShoppingGateway.ClinetSdk.Entities
{
    public class CreationAuditedEntity<TKey> : BaseEntity<TKey>
    {
        public DateTime? CreatedAt { get; set; }
        public string CreatorId { get; set; }

    }


    public class ModificationAuditedEntity<TKey> : CreationAuditedEntity<TKey>
    {
        public DateTime? ModifiedAt { get; set; }
        public string ModifierId { get; set; }

    }


    public class DeletionAuditedEntity<TKey> : ModificationAuditedEntity<TKey>
    {
        public DateTime? DeletedAt { get; set; }
        public string DeleterId { get; set; }
    }

    public class FullAuditedEntity<TKey> : DeletionAuditedEntity<TKey> { }
}
