using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public abstract class ProtocolElement
    {
        public abstract byte[] GetBytes();
        public abstract void SetBytes(byte[] bytes);
        public virtual int Count()
        {
            return 1;
        }
    }
}
