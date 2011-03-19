using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.Net.Protocols;
using Just.EventArguments;

namespace Just.Net.Specialized
{
    public class SyslogDaemon : UdpServer,ISyslogDaemon
    {
        public event EventHandler<GenericEventArgs<SyslogProtocol>> SyslogMessageReceived;

        public SyslogDaemon(System.Net.IPEndPoint receiveEndPoint, Encoding encoding, int bufferSize = 131072)
            : base(receiveEndPoint, encoding, bufferSize)
        {

        }

        protected override void ProcessData(byte[] data, System.Net.EndPoint sender)
        {
            SyslogProtocol protocol = new SyslogProtocol();
            protocol.SetBytes(data);
            if (SyslogMessageReceived != null) SyslogMessageReceived(sender, new GenericEventArgs<SyslogProtocol>(protocol));
        }


        public void Start()
        {
            StartReceiving();
        }

        public void Stop()
        {
            StopReceiving();
        }
    }
}
