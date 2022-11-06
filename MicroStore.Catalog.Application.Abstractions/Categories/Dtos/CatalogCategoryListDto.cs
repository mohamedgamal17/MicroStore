using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class CatalogCategoryListDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
