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
    public class UdpServer : IServerStatus
    {
        private IPEndPoint _ReceiveEndPoint;
        protected Encoding _Encoding;
        private Socket _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private int _BufferSize;
        private bool _ShouldRun;
        public event EventHandler<GenricEventArgs<EndPoint>> ReceiverStarted;
        public event EventHandler<EventArgs> ReceiverStopped;
        public event EventHandler<NetworkDataEventArgs> DataReceived;
        public event EventHandler<NetworkDataEventArgs> DataSent;
        public event EventHandler<ExceptionEventArgs> ExceptionCatched;

        public UdpServer(IPEndPoint receiveEndPoint, Encoding encoding, int bufferSize = 512)
        {
            this._ReceiveEndPoint = receiveEndPoint;
            this._Encoding = encoding;
            this._BufferSize = bufferSize;
        }

        public void StartReceiving()
        {
            this._ShouldRun = true;
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveLoop));
            receiveThread.IsBackground = true;
            receiveThread.Start(this._ReceiveEndPoint);
        }

        public void StopReceiving()
        {
            this._ShouldRun = false;
        }

        private void ReceiveLoop(object objEndPoint)
        {
            EndPoint endPoint = (EndPoint)objEndPoint;
            Socket receiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                receiver.Bind(endPoint);
            }
            catch (Exception ex)
            {
                ExceptionCatched(this, new ExceptionEventArgs(ex));
            }
            if (ReceiverStarted != null) ReceiverStarted(this, new GenricEventArgs<EndPoint>(endPoint));
            while (this._ShouldRun)
            {
                try
                {
                    if (receiver.Available > 0)
                    {
                        EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] buffer = new byte[this._BufferSize];
                        int count = receiver.ReceiveFrom(buffer, ref remoteEndPoint);
                        Array.Resize(ref buffer, count);
                        BeginProcessingData(buffer, remoteEndPoint);
                    }
                }
                catch (Exception ex)
                {
                    this._ShouldRun = false;
                    if (ReceiverStarted != null) ExceptionCatched(this, new ExceptionEventArgs(ex));
                }
            }
            if (ReceiverStarted != null) ReceiverStopped(this, new EventArgs());
        }

        protected void SendTo(byte[] data, EndPoint endPoint)
        {
            try
            {
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                bool isTooBig = false;
                byte[] toSend;
                if (data.Length > _BufferSize)
                {
                    toSend = new byte[_BufferSize];
                    Array.Copy(data, toSend, _BufferSize);
                    int length = data.Length - _BufferSize;
                    Array.Copy(data, _BufferSize, data, 0, length);
                    Array.Resize(ref data, length);
                    isTooBig = true;

                }
                else
                {
                    toSend = data;
                }

                sender.SendTo(toSend, endPoint);
                if (ReceiverStarted != null) DataSent(this, new NetworkDataEventArgs(toSend, endPoint));

                if (isTooBig)
                {
                    SendTo(data, endPoint);
                }
            }
            catch (Exception ex)
            {
                if (ReceiverStarted != null) ExceptionCatched(this, new ExceptionEventArgs(ex));
            }
        }


        private void BeginProcessingData(byte[] data, EndPoint sender)
        {
            Thread worker = new Thread(new ParameterizedThreadStart(StartProcessingData));
            worker.IsBackground = true;
            object[] objs = new object[2] { data, sender };
            worker.Start(objs);
        }

        private void StartProcessingData(object objs)
        {
            object[] objArray = (object[])objs;
            byte[] data = (byte[])objArray[0];
            EndPoint sender = (EndPoint)objArray[1];
            DataReceived(this, new NetworkDataEventArgs(data, sender));
            ProcessData(data, sender);
        }

        protected virtual void ProcessData(byte[] data, EndPoint sender)
        {
            
        }

        public IPEndPoint IPEndpoint 
        {
            get
            {
                return this._ReceiveEndPoint;
            }
        }
    }
}
