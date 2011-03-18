using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogAppname : ProtocolElement
    {
        public string Value { get; set; }

        public SyslogAppname(string appname)
        {
            this.Value = appname;
        }

        public SyslogAppname()
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
