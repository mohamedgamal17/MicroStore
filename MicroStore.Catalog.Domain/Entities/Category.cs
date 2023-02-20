
#pragma warning disable CS8618
using Volo.Abp.Domain.Entities;
namespace MicroStore.Catalog.Domain.Entities
{
    public class Category : BasicAggregateRoot<string>
    {

        public string Name { get;  set; }
        public string Description { get;  set; } = string.Empty;


        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }
  
     
    }
}
