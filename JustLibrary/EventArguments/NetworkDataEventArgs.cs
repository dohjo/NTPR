using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just
{
    namespace EventArguments
    {
        public class NetworkDataEventArgs : EventArgs
        {
            public byte[] Data { get; private set; }
            public System.Net.EndPoint EndPoint { get; private set; }

            public NetworkDataEventArgs(byte[] data,System.Net.EndPoint endPoint)
            {
                this.Data = data;
                this.EndPoint = endPoint;
            }
        }
    }
}