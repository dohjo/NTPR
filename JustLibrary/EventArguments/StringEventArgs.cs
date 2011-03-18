using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.EventArguments
{
    public class StringEventArgs : EventArgs
    {
        public string Value { get; set; }

        public StringEventArgs(string value)
        {
            this.Value = value;
        }
    }
}
