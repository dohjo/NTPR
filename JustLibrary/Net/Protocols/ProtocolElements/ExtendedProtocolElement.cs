using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public abstract class ExtendedProtocolElement : ProtocolElement
    {
        protected ProtocolElement[] Elements { get; set; }
        public byte Seperator { get; protected set; }

        public ExtendedProtocolElement(byte seperator)
        {
            this.Seperator = seperator;
        }

        public ExtendedProtocolElement()
        {
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
            int count = 0;
            foreach (ProtocolElement element in this.Elements)
            {
                bytes.AddRange(element.GetBytes());
                count++;
                if (this.Elements.Count() > count)
                {
                    bytes.Add(this.Seperator);
                }
            }

            return bytes.ToArray();
        }

        public override void SetBytes(byte[] bytes)
        {
            int start = 0;
            int end = 0;
            for (int ii = 0; ii < Elements.Length; ii++)
            {
                ProtocolElement element = Elements[ii];
                if (ii < Elements.Length - 1)
                {
                    if (element is ExtendedProtocolElement)
                    {
                        ExtendedProtocolElement xElement = (ExtendedProtocolElement)element;
                        if (xElement.Seperator == this.Seperator)
                        {
                            element.SetBytes(GetBytes(ref bytes, start, ref end, this.Seperator, xElement.Count()));
                        }
                        else
                        {
                            element.SetBytes(GetBytes(ref bytes, start, ref end, this.Seperator));
                        }
                    }
                    else
                    {
                        //ZU LETZT: Problem mit Seperator in Message (leerzeichen können in der nachricht sein)
                        element.SetBytes(GetBytes(ref bytes, start, ref end, this.Seperator));
                    }
                }
                else
                {
                    end = bytes.Length;
                    int length = end - start;
                    byte[] pass = new byte[length];
                    Array.Copy(bytes, start, pass, 0, end - start);
                    element.SetBytes(pass);
                }
                start = end + 1;
            }
        }

        protected byte[] GetBytes(ref byte[] bytes, int start, ref int end, byte seperator, int skip = 0)
        {
            int offset = 0;
            if (skip > 0)
            {
                for (int ii = 0; ii < skip; ii++)
                {
                    end = Array.IndexOf(bytes, seperator, start + offset);
                    offset = end + 1;
                }
            }
            else
            {
                end = Array.IndexOf(bytes, seperator, start);
                if (end < 0)
                {
                    end = bytes.Length;
                }
            }
            int length = end - start;
            byte[] pass = new byte[length];
            Array.Copy(bytes, start, pass, 0, end - start);
            return pass;
        }
    }
}
