namespace JiksGame.Core.Managers
{
    public interface ISocketManager
    {
        Task ListeningAsync(CancellationToken cancellationToken);
        Task UpdateStateAsync();
        Task<bool> SendAsync(int clientId, string message);
        Task SendAllClientsAsync(string message);
        Task ResetAsync();
    }
}

