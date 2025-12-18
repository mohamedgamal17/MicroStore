namespace MicroStore.Catalog.Application.Operations
{
    public abstract class EntitySynchronizationEvent<TEntity>
    {
        protected EntitySynchronizationEvent(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; set; }

    }

    public class EntityCreatedEvent<TEntity> : EntitySynchronizationEvent<TEntity>
    {
        public EntityCreatedEvent(TEntity entity) : base(entity)
        {

        }
    }

    public class EntityUpdatedEvent<TEntity> : EntitySynchronizationEvent<TEntity>
    {
        public EntityUpdatedEvent(TEntity entity) : base(entity)
        {

        }
    }

    public class EntityDeletedEvent<TEntity> : EntitySynchronizationEvent<TEntity>
    {
        public EntityDeletedEvent(TEntity entity) : base(entity)
        {

        }
    }
}
