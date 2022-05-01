using JiksGame.Core.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace JiksGame.Core.Engines
{
    public class SocketEngine : BaseEngine
    {
        private readonly ISocketManager _socketManager;

        public SocketEngine(
            IServiceScopeFactory serviceScopeFactory,
            ISocketManager socketManager) : base(serviceScopeFactory)
        {
            _socketManager = socketManager;
        }

        public override async Task ExecuteOneCycleAsync(
            IServiceScope serviceScope,
            CancellationToken cancellationToken)
        {
            await _socketManager.UpdateStateAsync();
            await Task.Delay(20);
        }
    }
}
