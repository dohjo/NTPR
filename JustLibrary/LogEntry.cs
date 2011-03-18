using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just
{
    public class LogEntry
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }

        public LogEntry(string information, DateTime time)
        {

        }

        public LogEntry(string information)
        {
            this.Information = information;
            this.Time = DateTime.Now;
        }

        public override string ToString()
        {
            return "[" + this.Time.ToString("yyyy-MM-dd hh:mm:ss") + "]: " + this.Information;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryFormat">{0} = Information, {1} = Time</param>
        /// <param name="dateTimeFormat"></param>
        /// <returns></returns>
        public string ToString(string entryFormat, string dateTimeFormat)
        {
            return String.Format(entryFormat, this.Information, this.Time.ToString(dateTimeFormat));
        }
    }
}
