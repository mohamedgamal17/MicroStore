using System.Security.Cryptography;

namespace MicroStore.Bff.Shopping.Data
{
    public abstract class Entity<TId>
    {
        public TId Id { get; set; }
    }

    public class AuditiedEntity<TId> : Entity<TId>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
