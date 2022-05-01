namespace JiksGame.Core.Commands
{
    public abstract class BaseSystemCommand : ISystemCommand
    {
        public bool CanExecute { get; private set; }
        public abstract Task ExecuteAsync(IServiceProvider serviceProvider);
    }
}
