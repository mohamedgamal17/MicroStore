

using MassTransit;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application
{
    public class ConsumerUnitOfWorkFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
        where TMessage : class
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ConsumerUnitOfWorkFilter(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("unit-of-work");
        }

        public  async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            using var uow = _unitOfWorkManager.Begin(true);

            await next.Send(context);

            await uow.CompleteAsync();
        }
    }
}
