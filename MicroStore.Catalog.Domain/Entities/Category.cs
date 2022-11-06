
using Volo.Abp.Domain.Entities;


namespace MicroStore.Catalog.Domain.Entities
{
    public class Category : BasicAggregateRoot<Guid>
    {

        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public Category(string name)
            : base(Guid.NewGuid())
        {
            Name = name;
        }


        protected Category() // Require For EFCore
        {

        }

        public void SetCategoryDescription(string description)
        {
            if (description.IsNullOrEmpty()
                || Description == description)
            {
                return;
            }

            Description = description;
        }

        public void UpdateCategoryName(string name)
        {
            if (Name.IsNullOrEmpty()
                || Name == name)
            {
                return;
            }

            Name = name;

        }



        [Obsolete]
        public Category(string name, string description)
            : base(Guid.NewGuid())
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
