using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogTimestamp : ProtocolElement
    {
        public string Value { get; set; }

        public SyslogTimestamp(DateTime timestamp)
        {
            SetTimestamp(timestamp);
        }

        public SyslogTimestamp()
        {
        }

        public void SetTimestamp(DateTime timestamp)
        {
            this.Value = timestamp.ToString("yyyy-MM-dd") + "T" + timestamp.ToString("hh:mm:ss") + "." + timestamp.Millisecond.ToString() + "Z";
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
