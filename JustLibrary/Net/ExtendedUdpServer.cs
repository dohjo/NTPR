using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Just;
using Just.EventArguments;

namespace Just.Net
{
    /// <summary>
    /// Obsolete (Use instead ServerStatusLogger)
    /// </summary>
    public class ExtendedUdpServer : UdpServer
    {
        private Log _Serverlog = new Log("[{1}]: {0}", "yyyy-MM-dd hh:mm:ss");
        private bool _DetailedLogging;

        public event EventHandler<EventArguments.StringEventArgs> LogEntryAdded 
        {
            add
            {
                this._Serverlog.LogEntryAdded += value;
            }
            remove
            {
                this._Serverlog.LogEntryAdded -= value;
            }
        }

        public ExtendedUdpServer(IPEndPoint receiveEndPoint, Encoding encoding, int bufferSize = 512, bool detailedLogging = true)
            : base(receiveEndPoint, encoding, bufferSize)
        {
            this._DetailedLogging = detailedLogging;
            this.DataReceived += new EventHandler<EventArguments.NetworkDataEventArgs>(DataReceivedEventHandler);
            this.DataSent += new EventHandler<EventArguments.NetworkDataEventArgs>(DataSentEventHandler);
            this.ExceptionCatched += new EventHandler<EventArguments.ExceptionEventArgs>(ExceptionCatchedEventHandler);
            this.ReceiverStarted += new EventHandler<GenricEventArgs<EndPoint>>(ReceiverStartedEventHandler);
            this.ReceiverStopped += new EventHandler<EventArgs>(ReceiverStoppedEventHandler);
        }

        protected virtual void ReceiverStoppedEventHandler(object sender, EventArgs e)
        {
            this._Serverlog.AddLogEntry("Receiver gestoppt!");
        }

        protected virtual void ReceiverStartedEventHandler(object sender, EventArgs e)
        {

            this._Serverlog.AddLogEntry("Receiver gestartet! (" + this.IPEndpoint.ToString() + ")");
        }

        protected virtual void ExceptionCatchedEventHandler(object sender, EventArguments.ExceptionEventArgs e)
        {
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry("Es ist ein Fehler aufgetreten: " + e.Exception.Message);
            }
            else
            {
                this._Serverlog.AddLogEntry("Es ist ein Fehler aufgetreten!");
            }
        }

        protected virtual void DataSentEventHandler(object sender, EventArguments.NetworkDataEventArgs e)
        {
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry("Gesendet (" + e.Data.Length + " Byte) nach " + ((IPEndPoint)e.EndPoint).ToString() + ": " + this._Encoding.GetString(e.Data));
            }
            else
            {
                this._Serverlog.AddLogEntry("Gesendet (" + e.Data.Length + " Byte) nach " + ((IPEndPoint)e.EndPoint).ToString());
            }
        }

        protected virtual void DataReceivedEventHandler(object sender, EventArguments.NetworkDataEventArgs e)
        {
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry("Empfangen (" + e.Data.Length + " Byte) von " + ((IPEndPoint)e.EndPoint).ToString() + ": " + this._Encoding.GetString(e.Data));
            }
            else
            {
                this._Serverlog.AddLogEntry("Empfangen (" + e.Data.Length + " Byte) von " + ((IPEndPoint)e.EndPoint).ToString());
            }
        }
    }
}
