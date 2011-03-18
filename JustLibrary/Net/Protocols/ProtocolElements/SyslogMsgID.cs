using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogMsgID : ProtocolElement
    {
        public int Value { get; set; }

        public SyslogMsgID(int msgID)
        {
            this.Value = msgID;
        }

        public SyslogMsgID()
        {
        }

        public override byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(this.Value.ToString());
        }

        public override void SetBytes(byte[] bytes)
        {
            this.Value = bytes[0];
        }
    }
}
