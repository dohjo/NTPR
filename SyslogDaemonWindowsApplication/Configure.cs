using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Just.Net.Specialized;
namespace SyslogDaemonWindowsApplication
{
    public partial class Configure : Form
    {
        public Configure()
        {
            InitializeComponent();
            comboBox.SelectedIndex = 0;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int port = 0;
            bool logging = checkBoxLogging.Checked;
            int.TryParse(textBoxPort.Text, out port);
            
            IPEndPoint ep = new IPEndPoint(IPAddress.Any,port);
            ISyslogDaemon daemon;
            if (comboBox.SelectedItem.ToString() == "TCP")
            {
                daemon = new TcpSyslogDaemon(ep);
            }
            else
            {
                daemon = new SyslogDaemon(ep, Encoding.UTF8);
            }
            Main main = new Main(daemon, logging);
            main.Show();
            main.FormClosed += new FormClosedEventHandler(main_FormClosed);
            this.Hide();
        }

        void main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
