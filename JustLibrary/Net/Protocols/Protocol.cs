using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Just.Net.Protocols.ProtocolElements;
namespace Just.Net.Protocols
{
    public abstract class Protocol
    {
        protected abstract ProtocolElement[] Elements { get; set; }
        protected abstract byte[] Seperator { get; set; }

        public abstract byte[] GetBytes();
        public abstract void SetBytes(byte[] bytes);
        public virtual int Count()
        {
            int total = 0;
            foreach (ProtocolElement element in this.Elements)
            {
                total += element.Count();
            }
            return total;
        }
    }
}
