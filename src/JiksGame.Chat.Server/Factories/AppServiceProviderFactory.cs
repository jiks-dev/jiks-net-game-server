using JiksGame.Core.Engines;
using JiksGame.Core.Managers;
using JiksGame.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JiksGame.Chat.Server.Factories
{
    internal class AppServiceProviderFactory
    {
        private const int _portNumber = 11000;

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
              .ConfigureServices((hostContext, services) =>
              {
                  services.AddSingleton<ICommandEngineManager, CommandEngineManager>();
                  services.AddSingleton<ICommandExceptionManager, CommandExceptionManager>();
                  services.AddSingleton<CommandEngine>();
                  services.AddSingleton<CommandEngineRunningService>();
                  services.AddHostedService<CommandEngineRunningService>();
                  services.AddSingleton<ISocketManager, SocketManager>((service) =>
                  {
                      // TODO : (dh) Port number will be get by system config
                      return new SocketManager(_portNumber);
                  });
                  services.AddSingleton<SocketListeningEngine>();
                  services.AddHostedService<SocketListeningEngineRunningService>();
                  services.AddSingleton<SocketEngine>();
                  services.AddHostedService<SocketEngineRunningService>();
              });
        }
    }
}
