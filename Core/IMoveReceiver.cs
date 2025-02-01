using System;

namespace Chess.Core
{
    internal interface IMoveReceiver : IDisposable
    {
        public event EventHandler<(byte[], int)> OnMoveDataReceived;
        public event EventHandler OnConnectionEstablished;
        public event EventHandler OnMessageSent; 
        public void Start();
        public void Stop();
        public void Listen();
        public void Send(byte[] data);
    }
}
