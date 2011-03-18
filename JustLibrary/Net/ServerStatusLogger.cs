using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Just.EventArguments;

namespace Just.Net
{
    public class ServerStatusLogger
    {
        private Log _Serverlog = new Log("[{1}]: {0}", "yyyy-MM-dd hh:mm:ss");
        private Encoding _Encoding = Encoding.Default;
        private bool _DetailedLogging = true;
        private bool _ConsecutiveFileLogging;
        private string _ServerLogFile;

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

        public ServerStatusLogger(ref IServerStatus server, Encoding encoding, bool detailedLogging = true, bool consecutiveFileLogging = false, string file = "serverlog.txt")
        {
            this._ConsecutiveFileLogging = consecutiveFileLogging;
            this._ServerLogFile = file;
            this._Encoding = encoding;
            this._DetailedLogging = detailedLogging;

            if (this._ConsecutiveFileLogging)
            {
                this.LogEntryAdded += new EventHandler<StringEventArgs>(ServerLoggerLogEntryAdded);
            }

            server.DataReceived += new EventHandler<EventArguments.NetworkDataEventArgs>(DataReceivedEventHandler);
            server.DataSent += new EventHandler<EventArguments.NetworkDataEventArgs>(DataSentEventHandler);
            server.ExceptionCatched += new EventHandler<EventArguments.ExceptionEventArgs>(ExceptionCatchedEventHandler);
            server.ReceiverStarted += new EventHandler<GenricEventArgs<EndPoint>>(ReceiverStartedEventHandler);
            server.ReceiverStopped += new EventHandler<EventArgs>(ReceiverStoppedEventHandler);
        }

        public void WriteLogToFile(string file)
        {
            System.IO.File.AppendAllLines(this._ServerLogFile, this._Serverlog.GetEntries(), this._Encoding);
        }

        public void WriteLogToFile()
        {
            WriteLogToFile(this._ServerLogFile);
        }

        void ServerLoggerLogEntryAdded(object sender, StringEventArgs e)
        {
            System.IO.File.AppendAllText(this._ServerLogFile, e.Value + System.Environment.NewLine, this._Encoding);
        }

        protected virtual void ReceiverStoppedEventHandler(object sender, EventArgs e)
        {
            this._Serverlog.AddLogEntry("Receiver gestoppt!");
        }

        protected virtual void ReceiverStartedEventHandler(object sender, GenricEventArgs<EndPoint> e)
        {

            this._Serverlog.AddLogEntry("Receiver gestartet! (" + ((System.Net.IPEndPoint)e.Value).ToString() + ")");
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
