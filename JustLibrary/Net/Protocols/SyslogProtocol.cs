using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.Net.Protocols.ProtocolElements;

namespace Just.Net.Protocols
{
    /// <summary>
    /// ToDo:
    ///  * SetBytes
    ///  * SyslogPriorityVersion GetBytes
    ///  * StructuredData + Elements
    ///  * Message + Elements
    /// </summary>
    public class SyslogProtocol : Protocol
    {
        public SyslogProtocol()
        {
            this.Seperator = Encoding.ASCII.GetBytes(SyslogValues.Seperator.ToString()).Single();
            Elements = new ProtocolElement[3];
            SyslogHeader header = new SyslogHeader(this.Seperator);
            header.Version = 1;
            Elements[0] = header;
            Elements[1] = new SyslogStructuredData();
            Elements[2] = new SyslogMessage();
        }

        public SyslogHeader Header 
        {
            get
            {
                return (SyslogHeader)Elements[0];
            }
            set
            {
                Elements[0] = value;
            }
        }
        public string Message 
        {
            get
            {
                return ((SyslogMessage)Elements[2]).Value;
            }
            set
            {
                ((SyslogMessage)Elements[2]).Value = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}{1}{2}{1}{3}",Elements[0].ToString(),Encoding.ASCII.GetString(new byte[] {this.Seperator}), Elements[1].ToString(), Elements[2].ToString());
        }
    }
}
