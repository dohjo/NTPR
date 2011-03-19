using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogValueElement : ProtocolElement
    {
        private string _Value = "";

        public string Value
        {
            get
            {
                if (this._Value == "")
                {
                    return SyslogValues.Nilvalue.ToString();
                }
                else
                {
                    return this._Value;
                }
            }
            set
            {
                this._Value = value;
            }
        }

        public SyslogValueElement(string value)
        {
            this.Value = value;
        }

        public SyslogValueElement()
        {
        }

        public override byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(this.Value);
        }

        public override void SetBytes(byte[] bytes)
        {
            this.Value = Encoding.ASCII.GetString(bytes);
        }
    }
}
