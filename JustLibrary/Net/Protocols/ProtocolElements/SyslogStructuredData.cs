using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    /// <summary>
    /// Not implemented
    /// </summary>
    public class SyslogStructuredData : ExtendedProtocolElement
    {
        public override byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(SyslogValues.Nilvalue.ToString());
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(GetBytes());
        }

        public override void SetBytes(byte[] bytes)
        {
            
        }
    }
}
