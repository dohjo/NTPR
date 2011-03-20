using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Just.EventArguments;

namespace Just.Net
{
    /// <summary>
    /// Logging Queue für ConsecutiveFileLogging
    /// </summary>
    public class ServerStatusLogger
    {
        private Log _Serverlog = new Log("[{1}]: {0}", "yyyy-MM-dd hh:mm:ss");
        private Encoding _Encoding = Encoding.Default;
        private bool _DetailedLogging = true;
        private bool _ConsecutiveFileLogging;
        private string _ServerLogFile;
        private IServerStatus _Server;

        public event EventHandler<EventArguments.StringEventArgs> StatusChanged;
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

        public void SetServer(ref IServerStatus server)
        {
            if (this._Server != null)
            {
                this._Server.DataReceived -= new EventHandler<EventArguments.NetworkDataEventArgs>(DataReceivedEventHandler);
                this._Server.DataSent -= new EventHandler<EventArguments.NetworkDataEventArgs>(DataSentEventHandler);
                this._Server.ExceptionCatched -= new EventHandler<EventArguments.ExceptionEventArgs>(ExceptionCatchedEventHandler);
                this._Server.ReceiverStarted -= new EventHandler<GenericEventArgs<EndPoint>>(ReceiverStartedEventHandler);
                this._Server.ReceiverStopped -= new EventHandler<EventArgs>(ReceiverStoppedEventHandler);
            }
            this._Server = server;
            this._Server.DataReceived += new EventHandler<EventArguments.NetworkDataEventArgs>(DataReceivedEventHandler);
            this._Server.DataSent += new EventHandler<EventArguments.NetworkDataEventArgs>(DataSentEventHandler);
            this._Server.ExceptionCatched += new EventHandler<EventArguments.ExceptionEventArgs>(ExceptionCatchedEventHandler);
            this._Server.ReceiverStarted += new EventHandler<GenericEventArgs<EndPoint>>(ReceiverStartedEventHandler);
            this._Server.ReceiverStopped += new EventHandler<EventArgs>(ReceiverStoppedEventHandler);
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

            SetServer(ref server);
        }

        public ServerStatusLogger(Encoding encoding, bool detailedLogging = true, bool consecutiveFileLogging = false, string file = "serverlog.txt")
        {
            this._ConsecutiveFileLogging = consecutiveFileLogging;
            this._ServerLogFile = file;
            this._Encoding = encoding;
            this._DetailedLogging = detailedLogging;

            if (this._ConsecutiveFileLogging)
            {
                this.LogEntryAdded += new EventHandler<StringEventArgs>(ServerLoggerLogEntryAdded);
            }
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
            string message = "Receiver gestoppt!";
            this._Serverlog.AddLogEntry(message);
            UpdateStatus(message);
        }

        protected virtual void ReceiverStartedEventHandler(object sender, GenericEventArgs<EndPoint> e)
        {
            string message = "Receiver gestartet! (" + ((System.Net.IPEndPoint)e.Value).ToString() + ")";
            this._Serverlog.AddLogEntry(message);
            UpdateStatus(message);
        }

        protected virtual void ExceptionCatchedEventHandler(object sender, EventArguments.ExceptionEventArgs e)
        {
            string message = "Es ist ein Fehler aufgetreten!";
            string detail = "Es ist ein Fehler aufgetreten: " + e.Exception.Message;
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry(detail);
            }
            else
            {
                this._Serverlog.AddLogEntry(message);
            }
            UpdateStatus(message);
        }

        protected virtual void DataSentEventHandler(object sender, EventArguments.NetworkDataEventArgs e)
        {
            string message = "Gesendet (" + e.Data.Length + " Byte) nach " + ((IPEndPoint)e.EndPoint).ToString();
            string detail = "Gesendet (" + e.Data.Length + " Byte) nach " + ((IPEndPoint)e.EndPoint).ToString() + ": " + this._Encoding.GetString(e.Data);
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry(detail);
            }
            else
            {
                this._Serverlog.AddLogEntry(message);
            }
            UpdateStatus(message);
        }

        protected virtual void DataReceivedEventHandler(object sender, EventArguments.NetworkDataEventArgs e)
        {
            string message = "Empfangen (" + e.Data.Length + " Byte) von " + ((IPEndPoint)e.EndPoint).ToString();
            string detail = "Empfangen (" + e.Data.Length + " Byte) von " + ((IPEndPoint)e.EndPoint).ToString() + ": " + this._Encoding.GetString(e.Data);
            if (this._DetailedLogging)
            {
                this._Serverlog.AddLogEntry(detail);
            }
            else
            {
                this._Serverlog.AddLogEntry(message);
            }
            UpdateStatus(message);
        }

        protected void UpdateStatus(string status)
        {
            if (this.StatusChanged != null) this.StatusChanged(this, new StringEventArgs(status));
            Status = status;
        }

        public string Status { get; set; }
    }
}
