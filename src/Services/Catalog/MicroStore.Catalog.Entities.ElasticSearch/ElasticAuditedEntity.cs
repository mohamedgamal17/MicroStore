namespace MicroStore.Catalog.Entities.ElasticSearch
{

    public class ElasticCreationAuditedEntity : ElasticEntity
    {
        public DateTime CreationTime { get; set; }
        public string CreatorId { get; set; }
    }

    public class ElasticModificationAuditedEntity : ElasticCreationAuditedEntity
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual string LastModifierId { get; set; }
    }

    public class ElasticAuditedEntity : ElasticModificationAuditedEntity
    {
        public virtual bool IsDeleted { get; set; }
        public virtual string DeleterId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }

    }
}
