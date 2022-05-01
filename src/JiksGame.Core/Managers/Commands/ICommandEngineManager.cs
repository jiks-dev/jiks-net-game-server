using JiksGame.Core.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace JiksGame.Core.Managers
{
    public interface ICommandEngineManager
    {
        Task AddCommandAsync(ISystemCommand command);
        Task ExecuteCommandAsync(IServiceScope serviceScope);
        Task ResetAsync();
    }
}
