using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogTimestamp : SyslogValueElement
    {
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
    }
}
