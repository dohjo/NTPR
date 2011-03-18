using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogHostname : ProtocolElement
    {
        public string Value { get; set; }

        public SyslogHostname(string hostname)
        {
            this.Value = hostname;
        }

        public SyslogHostname()
        {
        }

        public override byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(this.Value);
        }

        public override void SetBytes(byte[] bytes)
        {
            this.Value = Encoding.ASCII.GetString(bytes);
        }
    }
}
