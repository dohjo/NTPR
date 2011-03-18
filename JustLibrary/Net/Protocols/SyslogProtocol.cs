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
        protected override ProtocolElement[] Elements { get; set; }

        protected override byte[] Seperator { get; set; }

        public SyslogProtocol()
        {
            Elements = new ProtocolElement[3];
            Elements[0] = new SyslogHeader(this.Seperator);
            //Structured-Data
            //Message
        }

        public SyslogHeader Header { get; set; }

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override void SetBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
