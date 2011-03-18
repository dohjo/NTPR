using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.EventArguments
{
    public class GenricEventArgs<T> : EventArgs
    {
        public T Value { get; private set; }

        public GenricEventArgs(T value)
        {
            this.Value = value;
        }
    }
}
