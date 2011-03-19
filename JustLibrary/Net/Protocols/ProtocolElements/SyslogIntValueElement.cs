using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogIntValueElement : SyslogValueElement
    {
        public int ToInt()
        {
            int returnVal;
            int.TryParse(this.Value, out returnVal);
            return returnVal;
        }
    }
}
