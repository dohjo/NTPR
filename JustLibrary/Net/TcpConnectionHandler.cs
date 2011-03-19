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
    public class TcpConnectionHandler
    {
        public bool ShouldReceive { get; set; }
        private Socket _Socket;
        private int _BufferSize;
        public event EventHandler<NetworkDataEventArgs> DataReceived;
        public event EventHandler<NetworkDataEventArgs> DataSent;
        public event EventHandler<GenericEventArgs<EndPoint>> ReceiverStarted;
        public event EventHandler<EventArgs> ReceiverStopped;

        public TcpConnectionHandler(IPEndPoint endpoint, int bufferSize = 512)
        {
            this._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this._BufferSize = bufferSize;
        }

        public TcpConnectionHandler(Socket socket, int bufferSize = 512)
        {
            if (socket.ProtocolType != ProtocolType.Tcp)
            {
                throw new ArgumentException("Invalid Protocoltype (" + socket.ProtocolType.ToString() + ")");
            }
            else
            {
                this._Socket = socket;
                this._BufferSize = bufferSize;
            }
        }

        public void StartReceiving()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ReceiverLoop));
            thread.IsBackground = true;
            ShouldReceive = true;
            thread.Start(this._Socket);
        }

        private void ReceiverLoop(object obj)
        {
            
            Socket socket = (Socket)obj;
            if (ReceiverStarted != null) ReceiverStarted(this, new GenericEventArgs<EndPoint>(socket.RemoteEndPoint));
            while (ShouldReceive)
            {
                try
                {
                    if (socket.Available > 0)
                    {
                        byte[] buffer = new byte[_BufferSize];
                        int count = socket.Receive(buffer);
                        Array.Resize(ref buffer, count);
                        if(this.DataReceived != null) DataReceived(this,new NetworkDataEventArgs(buffer,socket.RemoteEndPoint));
                    }
                }
                catch (Exception ex)
                {
                    ShouldReceive = false;
                }
            }
            if (ReceiverStarted != null) ReceiverStopped(this, new EventArgs());
        }

        public void Send(string data, Encoding encoding)
        {
            Send(encoding.GetBytes(data));
        }

        public void Send(byte[] data)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(Sender));
            thread.IsBackground = true;
            object[] objsArray = new object[2] { this._Socket, data };
            thread.Start(objsArray);
        }

        private void Sender(object objs)
        {
            object[] objArray = (object[])objs;
            Socket socket = (Socket)objArray[0];
            byte[] data = (byte[])objArray[1];
            if (data.Length < _BufferSize)
            {
                socket.Send(data);
                if (this.DataSent != null) this.DataSent(this, new NetworkDataEventArgs(data, socket.RemoteEndPoint));
            }
            else
            {
                throw new ArgumentException("Die Nachricht ist zu groß: " + data.Length + " bytes");
            }
        }
    }
}
