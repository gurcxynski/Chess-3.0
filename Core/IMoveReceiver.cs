using Chess.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Core
{
    internal interface IMoveReceiver : IDisposable
    {
        public event EventHandler<string> OnMoveDataReceived;
        public event EventHandler OnConnectionEstablished;
        public event EventHandler OnMessageSent;
        public void ListenIncoming();
        public void Start();
        public void Stop();
    }
}
