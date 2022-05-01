using JiksGame.Core.Models;
using System.Net;
using System.Net.Sockets;

namespace JiksGame.Core.Managers
{
    public class SocketManager : ISocketManager
    {
        private object _lock = new object();
        private Socket _server;
        private List<SocketCommunicator> _clients = new List<SocketCommunicator>();
        private ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        public SocketManager(int port)
        {
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];
            var localEndPoint = new IPEndPoint(ipAddress, port);
            _server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(localEndPoint);
            _server.Listen(100);   
        }

        public async Task ListeningAsync(CancellationToken cancellationToken)
        {
            var isCompleteTask = false;

            var workTask = Task.Run(() =>
            {
                _manualResetEvent.Reset();
                _server.BeginAccept(new AsyncCallback(AcceptCallback), _server);
                _manualResetEvent.WaitOne();
                isCompleteTask = true;
            });

            var cancleTask = Task.Run(async() => 
            {
                while (true) 
                {
                    if (cancellationToken.IsCancellationRequested) 
                    {
                        break;
                    }

                    if (isCompleteTask) 
                    {
                        break;
                    }
                    await Task.Delay(1000);
                }
            }, cancellationToken);

            await Task.WhenAny(workTask, cancleTask);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            _manualResetEvent.Set();

            var server = (Socket)ar.AsyncState;
            var client = server.EndAccept(ar);

            AddClient(client);
        }

        public async Task UpdateStateAsync()
        {
            await CollectClientMessageAndTranslationAsync();

            RemoveDeadClients();
        }

        private async Task CollectClientMessageAndTranslationAsync() 
        {
            var clients  = GetAllClinets();

            foreach (var client in clients)
            {
                // TODO : (dh) Communication translation policy should be put in this position.
                var messages = client.PopReceivedMessages();
                foreach (var message in messages) 
                {
                    await SendAllClientsAsync(message);
                }
            }
        }


        private void RemoveDeadClients()
        {
            var removeClients = GetAllClinets().Where(row => row.IsNeedDisposed());
            if (removeClients.Count() < 1) 
            {
                return;
            }

            lock (_lock)
            {
                foreach (var removeClient in removeClients)
                {
                    _clients.Remove(removeClient);
                }
            }
        }
        public async Task<bool> SendAsync(int clientId, string message)
        {
            var client = GetAllClinets().FirstOrDefault(row => row.GetUniqueId() == clientId);
            if (client == null) 
            {
                return false;
            }

            await client.SendAsync(message);

            return true;
        }

        public async Task SendAllClientsAsync(string message)
        {
            var clients = GetAllClinets();

            foreach (var client in clients)
            {
                await client.SendAsync(message);
            }
        }

        private SocketCommunicator[] GetAllClinets()
        {
            SocketCommunicator[] clients;

            lock (_lock)
            {
                clients = _clients.ToArray();
            }

            return clients;
        }

        private void AddClient(Socket socket)
        {
            uint uniqueId = 1;
            var communicator = new SocketCommunicator(uniqueId, socket);
            communicator.BeginReceive();

            lock (_lock)
            {
                _clients.Add(communicator);
            }
        }

        public Task ResetAsync()
        {
            var clients = GetAllClinets();
            foreach (var client in clients)
            {
                client.StopReceived();
            }

            RemoveDeadClients();
            return Task.CompletedTask;
        }
    }
}
