//using MassTransit;
//using Microsoft.Extensions.Logging;
//using Volo.Abp.Uow;

//namespace MicroStore.Inventory.Application
//{
//    public class ConsumerUnitOfWorkFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
//       where TMessage : class
//    {

//        private readonly ILogger<ConsumerUnitOfWorkFilter<TMessage>> _logger;
//        private readonly IUnitOfWorkManager _unitOfWorkManager;

//        public ConsumerUnitOfWorkFilter(IUnitOfWorkManager unitOfWorkManager, ILogger<ConsumerUnitOfWorkFilter<TMessage>> logger)
//        {
//            _unitOfWorkManager = unitOfWorkManager;
//            _logger = logger;
//        }

//        public void Probe(ProbeContext context)
//        {
//            context.CreateScope("unit-of-work");
//        }

//        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
//        {

//            using var uow = _unitOfWorkManager.Begin(true);

//            try
//            {

//                await next.Send(context);

//                await uow.CompleteAsync(context.CancellationToken);

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("UnExpected Exception Occured In {FilterName}", nameof(ConsumerUnitOfWorkFilter<TMessage>));
//                _logger.LogException(ex);
//                await uow.RollbackAsync();

//            }

//        }
//    }
//}
