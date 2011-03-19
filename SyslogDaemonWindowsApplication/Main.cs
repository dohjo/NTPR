using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Just.Net;
using Just.Net.Specialized;
using Just.Net.Protocols;

namespace SyslogDaemonWindowsApplication
{
    public partial class Main : Form
    {
        ServerStatusLogger _Logger;
        ISyslogDaemon _SyslogDaemon;
        delegate void DataCallBack(string[] values);

        public Main(ISyslogDaemon daemon, bool logging)
        {
            InitializeComponent();
            this._SyslogDaemon = daemon;
            IServerStatus status = (IServerStatus)daemon;
            this._Logger = new ServerStatusLogger(ref status, Encoding.Default, true, logging, "syslog.txt");
            this._Logger.StatusChanged += new EventHandler<Just.EventArguments.StringEventArgs>(_Logger_StatusChanged);
            this._SyslogDaemon.SyslogMessageReceived += new EventHandler<Just.EventArguments.GenericEventArgs<SyslogProtocol>>(_SyslogDaemon_SyslogMessageReceived);
            this._SyslogDaemon.Start();
            this.FormClosed += new FormClosedEventHandler(Main_FormClosed);
        }

        void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._SyslogDaemon.Stop();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        void _SyslogDaemon_SyslogMessageReceived(object sender, Just.EventArguments.GenericEventArgs<SyslogProtocol> e)
        {
            string[] values = new string[6] 
            { 
                e.Value.Header.Facility.ToString(),
                e.Value.Header.Severity.ToString(),
                e.Value.Header.Timestamp.ToString(),
                e.Value.Header.Hostname,
                e.Value.Header.Appname,
                e.Value.Message
            };
            AddToGridView(values);
        }

        private void AddToGridView(string[] values)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new DataCallBack(AddToGridView), new object[] {values});
            }
            else
            {
                dataGridView1.Rows.Add(values);
            }
        }


        void _Logger_StatusChanged(object sender, Just.EventArguments.StringEventArgs e)
        {
            toolStripStatusLabel.Text = e.Value;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
