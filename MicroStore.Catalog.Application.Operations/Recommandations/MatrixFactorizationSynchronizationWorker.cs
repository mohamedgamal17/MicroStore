using Hangfire;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.Application.Common;
using Volo.Abp.BackgroundWorkers.Hangfire;
namespace MicroStore.Catalog.Application.Operations.Recommandations
{
    public class MatrixFactorizationSynchronizationWorker : HangfireBackgroundWorkerBase
    {

        private readonly ICollaborativeFilterMLTrainer _collaborativeFilterMLTrainer;
        public MatrixFactorizationSynchronizationWorker(ICollaborativeFilterMLTrainer collaborativeFilterMLTrainer)
        {
            RecurringJobId = nameof(MatrixFactorizationSynchronizationWorker);
            CronExpression = Cron.Daily();
            _collaborativeFilterMLTrainer = collaborativeFilterMLTrainer;
        }
        public override async Task DoWorkAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation("Reindexing users product score");

            await _collaborativeFilterMLTrainer.ReindexAsync();

            Logger.LogInformation("Ended from reindeing users product score");
        }
    }
}
