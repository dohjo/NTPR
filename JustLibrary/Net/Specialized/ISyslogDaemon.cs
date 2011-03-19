using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.EventArguments;
using Just.Net.Protocols;

namespace Just.Net.Specialized
{
    public interface ISyslogDaemon : IServerStatus
    {
        event EventHandler<GenericEventArgs<SyslogProtocol>> SyslogMessageReceived;
        void Start();
        void Stop();
    }
}
