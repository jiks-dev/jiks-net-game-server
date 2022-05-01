using System.Net.Sockets;
using System.Text;

namespace JiksGame.Core.Models
{
    public class SocketCommunicator : IDisposable
    {
        private const int _bufferSize = 1024;
        private const int _timeoutMiliesecond = 3000;
        private readonly uint _uniqueId;
        private readonly Socket _socket;

        private bool _isNeedDisposed = false;
        private bool _disposed = false;

        private object _lock = new object();
        private List<string> _receivedMessages = new List<string>();

        public SocketCommunicator(uint uniqueId, Socket socket)
        {
            _uniqueId = uniqueId;
            _socket = socket;
        }

        #region IDisposable
        public void Dispose()
        {
            _disposed = true;
            _isNeedDisposed = false;
            _socket.Close();
            _socket.Dispose();
        }
        #endregion

        #region Status
        public uint GetUniqueId() => _uniqueId;
        public bool IsNeedDisposed() => _isNeedDisposed;
        public bool IsDisposed => _disposed;
        #endregion

        #region Communications
        public async Task<bool> SendAsync(string message)
        {
            if (_disposed)
            {
                return false;
            }

            if (_isNeedDisposed)
            {
                return false;
            }

            var task = Task.Run(() =>
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                _socket.Send(bytes);
            });

            if (await Task.WhenAny(task, Task.Delay(_timeoutMiliesecond)) != task)
            {
                return false;
            }

            return true;
        }

        public void BeginReceive()
        {
            var buffer = new byte[_bufferSize];
            _socket.BeginReceive(buffer, 0, _bufferSize, 0, new AsyncCallback(ReadCallback), buffer);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            if (_isNeedDisposed)
            {
                return;
            }

            int bytesRead = _socket.EndReceive(ar);
            if (bytesRead < 0)
            {
                _isNeedDisposed = true;
                return;
            }

            var buffer = ar.AsyncState as byte[];
            if (buffer == null)
            {
                throw new Exception();
            }

            var messageBuilder = new StringBuilder();
            var message = messageBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead)).ToString();

            if (message == "EOF")
            {
                _isNeedDisposed = true;
                return;
            }
            else if (_isNeedDisposed)
            {
                return;
            }

            lock (_lock)
            {
                _receivedMessages.Add(message);
            }

            BeginReceive();
        }

        public void StopReceived()
        {
            _isNeedDisposed = true;
        }
        #endregion

        #region Message Collect
        public bool CanPopMessages()
        {
            lock (_lock)
            {
                return _receivedMessages.Count > 0;
            }
        }

        public string[] PopReceivedMessages()
        {
            string[] receviedMessages;

            lock (_lock)
            {
                receviedMessages = _receivedMessages.ToArray();
                _receivedMessages.Clear();
            }

            return receviedMessages;
        }
        #endregion
    }
}
