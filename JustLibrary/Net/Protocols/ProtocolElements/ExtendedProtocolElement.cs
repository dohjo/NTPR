using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public abstract class ExtendedProtocolElement : ProtocolElement
    {
        protected abstract ProtocolElement[] Elements { get; set; }
        protected abstract byte[] Seperator { get; set; }

        public ExtendedProtocolElement(byte[] seperator)
        {
            this.Seperator = seperator;
        }

        public override int Count()
        {
            int total = 0;
            foreach (ProtocolElement element in this.Elements)
            {
                total += element.Count();
            }
            return total;
        }

        public override byte[] GetBytes()
        {
            List<byte> bytes = new List<byte>();

            foreach (ProtocolElement element in this.Elements)
            {
                bytes.AddRange(element.GetBytes());
                bytes.AddRange(this.Seperator);
            }

            return bytes.ToArray();
        }

        public override void SetBytes(byte[] bytes)
        {
            
        }
    }
}
