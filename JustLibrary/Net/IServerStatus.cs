using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Just.EventArguments;
namespace Just.Net
{
    public interface IServerStatus
    {
        event EventHandler<GenericEventArgs<EndPoint>> ReceiverStarted;
        event EventHandler<EventArgs> ReceiverStopped;
        event EventHandler<NetworkDataEventArgs> DataReceived;
        event EventHandler<NetworkDataEventArgs> DataSent;
        event EventHandler<ExceptionEventArgs> ExceptionCatched;
    }
}
