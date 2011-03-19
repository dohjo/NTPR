using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.Net.Protocols;
namespace SyslogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SyslogProtocol protocol = new SyslogProtocol();
            protocol.Header.Appname = "SyslogTest";
            protocol.Header.Facility = SyslogFacility.Local0;
            protocol.Header.Hostname = "127.0.0.1";
            protocol.Header.Severity = SyslogSeverity.Debug;
            protocol.Message = "Hallo! dies ist eine Testnachricht";
            protocol.Header.Timestamp = DateTime.Now;
            protocol.Header.Version = 1;
            protocol.Header.MsgID = "ID22";
            protocol.Header.ProcID = "Proc99";
            SyslogProtocol settedProtocol = new SyslogProtocol();
            settedProtocol.SetBytes(protocol.GetBytes());
            Console.WriteLine("Original: " + protocol.ToString());
            Console.WriteLine("Setted:   " + settedProtocol.ToString());
            Console.Read();
        }

        static void HeaderTest()
        {
            byte seperator = Encoding.ASCII.GetBytes(" ").Single();

            Just.Net.Protocols.ProtocolElements.SyslogHeader header = new Just.Net.Protocols.ProtocolElements.SyslogHeader(seperator)
            {
                Appname = "SyslogTest",
                Facility = SyslogFacility.Local0,
                Hostname = "Lokal",
                //MsgID = 13,
                //ProcID = 144,
                Severity = SyslogSeverity.Debug,
                Timestamp = DateTime.Now,
                Version = 1
            };

            Just.Net.Protocols.ProtocolElements.SyslogHeader settedHeader = new Just.Net.Protocols.ProtocolElements.SyslogHeader(seperator);
            settedHeader.SetBytes(header.GetBytes());

            Console.WriteLine("Original Header: " + Encoding.ASCII.GetString(header.GetBytes()));
            Console.WriteLine("Setted Header:   " + Encoding.ASCII.GetString(settedHeader.GetBytes()));
        }
    }
}
