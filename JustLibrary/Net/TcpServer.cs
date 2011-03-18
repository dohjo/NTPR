using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Just.EventArguments;

namespace Just.Net
{
    public class TcpServer : IServerStatus
    {
        List<TcpConnectionHandler> _Clients = new List<TcpConnectionHandler>();
        Socket _Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        EndPoint _EndPoint;
        private bool ShouldListen { get; set; }
        private int _MaxClients;
        private int _BufferSize;

        public event EventHandler<GenricEventArgs<EndPoint>> ReceiverStarted;
        public event EventHandler<EventArgs> ReceiverStopped;
        public event EventHandler<NetworkDataEventArgs> DataReceived;
        public event EventHandler<NetworkDataEventArgs> DataSent;
        public event EventHandler<ExceptionEventArgs> ExceptionCatched;

        public TcpServer(EndPoint listenEndpoint, int bufferSize = 512, int maxClients = 10)
        {
            this._BufferSize = bufferSize;
            this._MaxClients = maxClients;
            this._EndPoint = listenEndpoint;
        }

        public void StartListening()
        {
            ShouldListen = true;
            this._Listener.Bind(this._EndPoint);
            Thread thread = new Thread(new ParameterizedThreadStart(ListenLoop));
            thread.IsBackground = true;
            thread.Start(this._Listener);
        }

        private void ListenLoop(object obj)
        {
            Socket listener = (Socket)obj;
            listener.Listen(_MaxClients);
            while (ShouldListen)
            {
                try
                {
                    TcpConnectionHandler tempClient = new TcpConnectionHandler(listener.Accept(),this._BufferSize);
                    tempClient.StartReceiving();
                    tempClient.DataReceived += new EventHandler<NetworkDataEventArgs>(DataReceivedHandler);
                    tempClient.ReceiverStarted += new EventHandler<GenricEventArgs<EndPoint>>(ClientReceiverStarted);
                    tempClient.ReceiverStopped += new EventHandler<EventArgs>(ClientReceiverStopped);
                    tempClient.DataSent += new EventHandler<NetworkDataEventArgs>(ClientDataSent);
                }
                catch (Exception ex)
                {
                    ShouldListen = false;
                    if (this.ExceptionCatched != null) this.ExceptionCatched(this, new ExceptionEventArgs(ex));
                }
            }
        }

        void ClientDataSent(object sender, NetworkDataEventArgs e)
        {
            if (this.DataSent != null) this.DataSent(sender, e);
        }

        void ClientReceiverStopped(object sender, EventArgs e)
        {
            if (this.ReceiverStopped != null) this.ReceiverStopped(sender, e);
        }

        void ClientReceiverStarted(object sender, GenricEventArgs<EndPoint> e)
        {
            if (this.ReceiverStarted != null) this.ReceiverStarted(sender, e);
        }

        public void StopListening()
        {
            ShouldListen = false;
        }

        private void DataReceivedHandler(object sender, NetworkDataEventArgs e)
        {
            if (DataReceived != null) DataReceived(sender, e);
            Thread thread = new Thread(new ParameterizedThreadStart(StartProcessingData));
            thread.IsBackground = true;
            object[] objs = new object[2] { e.Data, sender };
            thread.Start(objs);
        }

        private void StartProcessingData(object objs)
        {
            object[] objArray = (object[])objs;
            byte[] data = (byte[])objArray[0];
            TcpConnectionHandler handler = (TcpConnectionHandler)objArray[1];
            ProcessData(data, handler);
        }

        protected virtual void ProcessData(byte[] data, TcpConnectionHandler sender)
        {

        }
    }
}
