using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogHeader : ExtendedProtocolElement
    {
        public SyslogHeader(byte seperator)
            : base(seperator)
        {
            Elements = new ProtocolElement[6];
            Elements[0] = new SyslogPriorityVersion();
            Elements[1] = new SyslogTimestamp();
            Elements[2] = new SyslogHostname();
            Elements[3] = new SyslogAppname();
            Elements[4] = new SyslogProcID();
            Elements[5] = new SyslogMsgID();
        }

        public int Version 
        {
            get
            {
                return ((SyslogPriorityVersion)Elements[0]).Version;
            }
            set
            {
                ((SyslogPriorityVersion)Elements[0]).Version = value;
            }
        }

        public SyslogSeverity Severity
        {
            get
            {
                return ((SyslogPriorityVersion)Elements[0]).Severity;
            }
            set
            {
                ((SyslogPriorityVersion)Elements[0]).Severity = value;
            }
        }

        public SyslogFacility Facility
        {
            get
            {
                return ((SyslogPriorityVersion)Elements[0]).Facility;
            }
            set
            {
                ((SyslogPriorityVersion)Elements[0]).Facility = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return DateTime.Parse(((SyslogTimestamp)Elements[1]).Value);
            }
            set
            {
                ((SyslogTimestamp)Elements[1]).SetTimestamp(value);
            }
        }

        public string Hostname
        {
            get
            {
                return ((SyslogHostname)Elements[2]).Value;
            }
            set
            {
                ((SyslogHostname)Elements[2]).Value = value;
            }
        }

        public string Appname
        {
            get
            {
                return ((SyslogAppname)Elements[3]).Value;
            }
            set
            {
                ((SyslogAppname)Elements[3]).Value = value;
            }
        }

        public string ProcID
        {
            get
            {
                return ((SyslogProcID)Elements[4]).Value;
            }
            set
            {
                ((SyslogProcID)Elements[4]).Value = value;
            }
        }

        public string MsgID
        {
            get
            {
                return ((SyslogMsgID)Elements[5]).Value;
            }
            set
            {
                ((SyslogMsgID)Elements[5]).Value = value;
            }
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(GetBytes());
        }
    }
}
