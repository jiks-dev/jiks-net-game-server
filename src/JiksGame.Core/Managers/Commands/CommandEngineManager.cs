using JiksGame.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace JiksGame.Core.Managers
{
    public class CommandEngineManager : ICommandEngineManager
    {
        ConcurrentQueue<ISystemCommand> _commands = new ConcurrentQueue<ISystemCommand>();

        public Task AddCommandAsync(ISystemCommand command)
        {
            _commands.Enqueue(command);
            return Task.CompletedTask;
        }

        public async Task ExecuteCommandAsync(IServiceScope serviceScope)
        {
            bool isDequeue = _commands.TryDequeue(out var command);
            if (isDequeue == false)
            {
                return;
            }

            if (command == null)
            {
                return;
            }

            await command.ExecuteAsync(serviceScope.ServiceProvider);
        }

        public Task ResetAsync()
        {
            _commands.Clear();
            return Task.CompletedTask;
        }
    }
}
