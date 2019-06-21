using GodSharp.Sockets.Abstractions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace GodSharp.Sockets.Tcp
{
    internal sealed class TcpListener : NetListener<ITcpConnection>, INetListener<ITcpConnection>, ITcpListener, IDisposable
    {
        public TcpListener(ITcpConnection connection) : base(connection)
        {
        }
        
        protected override void OnBeginReceive(ref byte[] buffers) => Connection.Instance.BeginReceive(buffers, 0, buffers.Length, SocketFlags.None, ReceivedCallback, null);

        protected override T OnEndReceive<T>(IAsyncResult result) => new ReceiveResult(Connection.Instance.EndReceive(result, out SocketError error), Connection.RemoteEndPoint) as T;

        protected override void OnReceiveHandling(byte[] buffers, IPEndPoint remote = null, IPEndPoint local = null)
        {
            if (Connection.OnReceived != null)
            {
                Task.Run(() => Connection.OnReceived(new NetClientReceivedEventArgs<ITcpConnection>(Connection, buffers, remote, local)));
            }
        }

        protected override void OnStop(Exception exception)
        {
            if (Connection.OnDisconnected != null)
            {
                Task.Run(() => Connection.OnDisconnected(new NetClientEventArgs<ITcpConnection>(Connection) { Exception = exception }));
            }
        }

        protected override void OnException(Exception exception)
        {
            if (Connection.OnException != null)
            {
                Task.Run(() => Connection.OnException(new NetClientEventArgs<ITcpConnection>(Connection) { Exception = exception }));
            }            
        }

        public override void Dispose()
        {
            Connection = null;
        }
    }
}
