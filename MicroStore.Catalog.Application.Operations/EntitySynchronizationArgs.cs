namespace MicroStore.Catalog.Application.Operations
{
    public abstract class EntitySynchronizationArgs<TEntity>
    {
        protected EntitySynchronizationArgs(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; set; }

    }

    public class EntityCreatedArgs<TEntity> : EntitySynchronizationArgs<TEntity>
    {
        public EntityCreatedArgs(TEntity entity) : base(entity)
        {

        }
    }

    public class EntityUpdatedArgs<TEntity> : EntitySynchronizationArgs<TEntity>
    {
        public EntityUpdatedArgs(TEntity entity) : base(entity)
        {

        }
    }

    public class EntityDeletedArgs<TEntity> : EntitySynchronizationArgs<TEntity>
    {
        public EntityDeletedArgs(TEntity entity) : base(entity)
        {

        }
    }
}
