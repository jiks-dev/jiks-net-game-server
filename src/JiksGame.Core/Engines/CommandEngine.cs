using JiksGame.Core.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace JiksGame.Core.Engines
{
    public class CommandEngine : BaseEngine
    {
        private readonly ICommandEngineManager _commandEngineManager;

        public CommandEngine(
            IServiceScopeFactory serviceScopeFactory,
            ICommandEngineManager commandEngineManager) 
            : base(serviceScopeFactory)
        {
            _commandEngineManager = commandEngineManager;
        }

        public override async Task ExecuteOneCycleAsync(
            IServiceScope serviceScope, 
            CancellationToken cancellationToken)
        {
            await _commandEngineManager.ExecuteCommandAsync(serviceScope);
        }

        public override async Task ResetAsync()
        {
            await _commandEngineManager.ResetAsync();
        }
    }
}
