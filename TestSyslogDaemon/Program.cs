using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.Net.Specialized;
using Just.Net;

namespace TestSyslogDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.IPEndPoint ep = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 4004);
            SyslogDaemon daemon = new SyslogDaemon(ep, Encoding.UTF8);
            IServerStatus istatus = (IServerStatus)daemon;
            ServerStatusLogger logger = new ServerStatusLogger(ref istatus, Encoding.UTF8);
            logger.LogEntryAdded += new EventHandler<Just.EventArguments.StringEventArgs>(logger_LogEntryAdded);
            daemon.StartReceiving();
            Console.Read();
        }

        static void logger_LogEntryAdded(object sender, Just.EventArguments.StringEventArgs e)
        {
            Console.WriteLine(e.Value);
        }
    }
}
