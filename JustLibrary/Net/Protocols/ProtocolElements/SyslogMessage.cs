using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogMessage : SyslogValueElement
    {
        public Encoding Encoding { get; set;  }

        public SyslogMessage(Encoding encoding)
        {
            this.Encoding = encoding;
        }

        public SyslogMessage()
        {
            this.Encoding = Encoding.UTF8;
        }

        public override byte[] GetBytes()
        {
            if (this.Encoding == Encoding.UTF8)
            {
                byte[] encodedBytes = Encoding.GetBytes(this.Value);
                byte[] returnVal = new byte[encodedBytes.Length + SyslogValues.ByteOrderMarkUtf8.Length];
                Array.Copy(SyslogValues.ByteOrderMarkUtf8, returnVal,SyslogValues.ByteOrderMarkUtf8.Length);
                Array.Copy(encodedBytes, 0, returnVal, SyslogValues.ByteOrderMarkUtf8.Length, encodedBytes.Length);
                return returnVal;
            }
            else
            {
                return Encoding.GetBytes(this.Value);
            }
        }

        public override void SetBytes(byte[] bytes)
        {
            byte[] checkBom = new byte[SyslogValues.ByteOrderMarkUtf8.Length];
            Array.Copy(bytes, checkBom, checkBom.Length);
            bool isUtf8Encoded = true;

            for (int ii = 0; ii < checkBom.Length; ii++)
            {
                if (checkBom[ii] != SyslogValues.ByteOrderMarkUtf8[ii])
                {
                    isUtf8Encoded = false;
                }
            }

            if (isUtf8Encoded)
            {
                this.Encoding = Encoding.UTF8;
                byte[] message = new byte[bytes.Length - checkBom.Length];
                Array.Copy(bytes, checkBom.Length, message, 0, message.Length);
                this.Value = Encoding.UTF8.GetString(message);
            }
            else
            {
                this.Value = Encoding.GetString(bytes);
            }
            
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
