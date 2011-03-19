using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.Net.Protocols
{
    public static class SyslogValues
    {
        public static char Nilvalue
        {
            get
            {
                return '-';
            }
        }

        public static byte[] ByteOrderMarkUtf8
        {
            get
            {
                return new byte[3] { 0xEF, 0xBB, 0xBF } ;
            }
        }

        public static char Seperator
        {
            get
            {
                return ' ';
            }
        }
    }
}
