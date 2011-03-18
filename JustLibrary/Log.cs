using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just
{
    public class Log
    {
        List<LogEntry> _LogEntries = new List<LogEntry>();
        string _EntryFormat, _DateTimeFormat;
        public event EventHandler<EventArguments.StringEventArgs> LogEntryAdded;

        public Log()
        {
            this._EntryFormat = "[{1}]: {0}";
            this._DateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        }

        public Log(string entryFormat, string dateTimeFormat)
        {
            this._DateTimeFormat = dateTimeFormat;
            this._EntryFormat = entryFormat;
        }

        public void AddLogEntry(LogEntry entry)
        {
            this._LogEntries.Add(entry);
            if (this.LogEntryAdded != null) LogEntryAdded(this, new EventArguments.StringEventArgs(entry.ToString(this._EntryFormat, this._DateTimeFormat)));
        }

        public void AddLogEntry(string information)
        {
            AddLogEntry(new LogEntry(information));
        }

        public void AddLogEntry(string information, DateTime time)
        {
            AddLogEntry(new LogEntry(information, time));
        }

        public string[] GetEntries()
        {
            return GetEntries(this._EntryFormat, this._DateTimeFormat);
        }

        public string[] GetEntries(string entryFormat, string dateTimeFormat)
        {
            string[] entries = new string[this._LogEntries.Count];
            for (int ii = 0; ii < entries.Length; ii++)
            {
                entries[ii] = this._LogEntries[ii].ToString(entryFormat, dateTimeFormat);
            }
            return entries;
        }
    }
}
