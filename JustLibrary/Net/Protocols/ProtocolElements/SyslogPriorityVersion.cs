using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols.ProtocolElements
{
    public class SyslogPriorityVersion : ProtocolElement
    {        
        public int Version { get; set; }
        public SyslogFacility Facility { get; set; }
        public SyslogSeverity Severity { get; set; }


        public SyslogPriorityVersion(int version, SyslogFacility facility, SyslogSeverity severity)
        {
            this.Version = version;
            this.Severity = severity;
            this.Facility = facility;
        }

        public SyslogPriorityVersion()
        {
        }

        public override byte[] GetBytes()
        {
            int prival = (int)this.Facility * 8 + (int)this.Severity;
            //FALSCH
            /*
            byte[] bytes = new byte[4]
            {
                (byte)'<',
                (byte)prival,
                (byte)'>',
                Encoding.ASCII.GetBytes(this.Version.ToString())[0]
            };
            */

            //RICHTIG nach RFC5424
            byte[] bytes = Encoding.ASCII.GetBytes("<" + prival.ToString() + ">" + this.Version.ToString());
            
            return bytes;
        }

        public override void SetBytes(byte[] bytes)
        {
            //FALSCH: Prival muss aufgebaut sein asu %d48-%d57, d.h. nicht als byte wert
            /*
            int index = Array.FindIndex(bytes, e => e == (byte)'<') + 1;
            int prival = bytes[index];
            int facility = prival & 248;
            facility = facility / 8;
            int severity = prival & 7;
            index = Array.FindIndex(bytes, e => e == (byte)'>') + 1;
            string digit = Encoding.ASCII.GetString(new byte[1] { bytes[index] });
            int version;
            int.TryParse(digit, out version);
            this.Version = version;
            */

            //RICHTIG nach RFC5424
            string byteString = Encoding.ASCII.GetString(bytes);
            int prival = 0;
            int start = byteString.IndexOf('<');
            int length = byteString.IndexOf('>') - start;
            int.TryParse(byteString.Substring(start, length), out prival);
            int facility = prival & 248;
            facility = facility / 8;
            int severity = prival & 7;
            this.Severity = (SyslogSeverity)severity;
            this.Facility = (SyslogFacility)facility;
            int version = 0;
            int.TryParse(byteString.Substring(byteString.IndexOf('>') + 1), out version);
            this.Version = version;
        }
    }
}