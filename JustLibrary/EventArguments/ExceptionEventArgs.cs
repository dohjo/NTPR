using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just.EventArguments
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public ExceptionEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
    }
}
