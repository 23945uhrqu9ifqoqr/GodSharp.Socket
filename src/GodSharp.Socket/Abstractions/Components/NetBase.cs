using System;
using System.Threading.Tasks;

namespace GodSharp.Sockets.Abstractions
{
    public abstract class NetBase<TConnection,TEventArgs> : INetBase<TConnection>, IEvent<TConnection, TEventArgs>, IDisposable 
        where TConnection : INetConnection
        where TEventArgs : NetEventArgs
    {
        public virtual SocketEventHandler<NetClientEventArgs<TConnection>> OnConnected { get; set; }
        public SocketEventHandler<NetClientReceivedEventArgs<TConnection>> OnReceived { get; set; }
        public SocketEventHandler<NetClientEventArgs<TConnection>> OnDisconnected { get; set; }

        public SocketEventHandler<TEventArgs> OnStarted { get; set; }

        public SocketEventHandler<TEventArgs> OnStopped { get; set; }

        public SocketEventHandler<NetClientEventArgs<TConnection>> OnException { get; set; }
        
        public virtual int Id { get; internal set; }

        public virtual string Name { get; internal set; }

        public virtual string Key { get; internal set; }

        public virtual bool Running { get; protected set; }

        public abstract void Start();

        public abstract void Stop();

        protected virtual void OnConnectedHandler(NetClientEventArgs<TConnection> args)
        {
            if (OnConnected != null)
            {
                Task.Run(() => OnConnected(args));
            }
        }

        protected virtual void OnReceivedHandler(NetClientReceivedEventArgs<TConnection> args)
        {
            if (OnReceived != null)
            {
                Task.Run(() => OnReceived(args));
            }
        }

        protected virtual void OnDisconnectedHandler(NetClientEventArgs<TConnection> args)
        {
            if (OnDisconnected != null)
            {
                Task.Run(() => OnDisconnected(args));
            }
        }

        protected virtual void OnStartedHandler(TEventArgs args)
        {
            if (OnStarted != null)
            {
                Task.Run(() => OnStarted(args));
            }
        }

        protected virtual void OnStoppedHandler(TEventArgs args)
        {
            if (OnStopped != null)
            {
                Task.Run(() => OnStopped(args));
            }
        }

        protected virtual void OnExceptionHandler(NetClientEventArgs<TConnection> args)
        {
            if (OnException != null)
            {
                Task.Run(() => OnException(args));
            }
        }

        public abstract void Dispose();
    }
}
