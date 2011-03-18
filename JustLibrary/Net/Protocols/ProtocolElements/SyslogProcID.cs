using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogProcID : ProtocolElement
    {
        public int Value { get; set; }

        public SyslogProcID(int procID)
        {
            this.Value = procID;
        }

        public SyslogProcID()
        {
        }

        public override byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(Value.ToString());
        }

        public override void SetBytes(byte[] bytes)
        {
            this.Value = bytes[0];
        }
    }
}
