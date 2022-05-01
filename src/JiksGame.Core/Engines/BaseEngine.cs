using Microsoft.Extensions.DependencyInjection;

namespace JiksGame.Core.Engines
{
    public abstract class BaseEngine
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BaseEngine(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                using var serviceScope = _serviceScopeFactory.CreateScope();
                await ExecuteOneCycleAsync(serviceScope, cancellationToken);
                await Task.Delay(200, cancellationToken);
            }
        }

        public abstract Task ExecuteOneCycleAsync(IServiceScope serviceScope, CancellationToken cancellationToken);
        public virtual Task ResetAsync() 
        {
            return Task.CompletedTask;
        }
    }
}
