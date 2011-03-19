using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;   
using Just.Net.Protocols;

namespace Just.Net.Specialized
{
    public class TcpSyslogDaemon : TcpServer, ISyslogDaemon
    {
        public event EventHandler<Just.EventArguments.GenericEventArgs<SyslogProtocol>> SyslogMessageReceived;

        public TcpSyslogDaemon(IPEndPoint listenEndpoint, int bufferSize = 131072, int maxClients = 100)
            : base(listenEndpoint, bufferSize, maxClients)
        {
            
        }

        protected override void ProcessData(byte[] data, TcpConnectionHandler sender)
        {
            SyslogProtocol protocol = new SyslogProtocol();
            protocol.SetBytes(data);
            if (SyslogMessageReceived != null) SyslogMessageReceived(sender, new EventArguments.GenericEventArgs<SyslogProtocol>(protocol));
        }

        public void Start()
        {
            StartListening();
        }

        public void Stop()
        {
            StopListening();
        }
    }
}
