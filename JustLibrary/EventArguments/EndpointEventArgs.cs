using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.EventArguments
{
    public class EndPointEventArgs
    {
        public System.Net.EndPoint Value { get; private set; }

        public EndPointEventArgs(System.Net.EndPoint value)
        {
            this.Value = value;
        }
    }
}
