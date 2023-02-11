using Microsoft.AspNetCore.Mvc;

namespace MicroStore.BuildingBlocks.AspNetCore.Attributes
{
    public class CustomFromQueryAttribute : FromQueryAttribute
    {
        public CustomFromQueryAttribute(string name)
        {
           Name =  name.ToSnakeCase();
        }
    }
}
