namespace JiksGame.Core.Commands
{
    public interface ISystemCommand
    {
        Task ExecuteAsync(IServiceProvider serviceProvider);
    }
}
