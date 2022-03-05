using JiksGame.Core.Engines;
using JiksGame.Core.Managers;
using Microsoft.Extensions.Hosting;

namespace JiksGame.Core.Services
{
    public class CommandEngineRunningService : BackgroundService
    {
        private readonly CommandEngine _engine;
        private readonly ICommandExceptionManager _commandExceptionManager;

        public CommandEngineRunningService(
            CommandEngine engine,
            ICommandExceptionManager commandExceptionManager)
        {
            _engine = engine;
            _commandExceptionManager = commandExceptionManager;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false) 
            {
                try
                {
                    await _engine.ExecuteAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _commandExceptionManager.Add(e);
                }
                finally
                {
                    await _engine.ResetAsync();
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
