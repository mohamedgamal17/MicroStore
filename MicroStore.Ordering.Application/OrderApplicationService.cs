using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Ordering.Application
{
    public abstract class OrderApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();

    }
}
