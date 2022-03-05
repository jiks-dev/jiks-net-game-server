using JiksGame.Core.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace JiksGame.Core.Engines
{
    public class SocketListeningEngine : BaseEngine
    {
        private readonly ISocketManager _socketManager;

        public SocketListeningEngine(
            IServiceScopeFactory serviceScopeFactory,
            ISocketManager socketManager) : base(serviceScopeFactory)
        {
            _socketManager = socketManager;
        }

        public override async Task ExecuteOneCycleAsync(
            IServiceScope serviceScope,
            CancellationToken cancellationToken)
        {
            await _socketManager.ListeningAsync(cancellationToken);
        }
    }
}
