using JiksGame.Core.Engines;
using Microsoft.Extensions.Hosting;

namespace JiksGame.Core.Services
{
    public class SocketListeningEngineRunningService : BackgroundService
    {
        private readonly SocketListeningEngine _engine;
        
        public SocketListeningEngineRunningService(SocketListeningEngine engine)
        {
            _engine = engine;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false) 
            {
                try
                {
                    await _engine.ExecuteAsync(cancellationToken);
                }
                catch (Exception)
                {
                }
                finally
                {
                    await _engine.ResetAsync();
                }

                await Task.Delay(2000, cancellationToken);
            }
        }
    }
}
