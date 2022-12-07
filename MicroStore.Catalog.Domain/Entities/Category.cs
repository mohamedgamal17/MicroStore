
using Volo.Abp.Domain.Entities;


namespace MicroStore.Catalog.Domain.Entities
{
    public class Category : BasicAggregateRoot<Guid>
    {

        public string Name { get;  set; }
        public string Description { get;  set; } = string.Empty;


        private Category()
        {

        }
        public Category(string name)
            : base(Guid.NewGuid())
        {
            Name = name;
        }
     
    }
}
