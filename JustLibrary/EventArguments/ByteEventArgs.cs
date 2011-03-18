using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just
{
    namespace EventArguments
    {
        public class ByteEventArgs : EventArgs
        {
            public byte[] Data { get; private set; }

            public ByteEventArgs(byte[] data)
            {
                this.Data = data;
            }
        }
    }
}