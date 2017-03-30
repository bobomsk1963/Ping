using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
namespace EventServer
{
    public partial class FormEventServer : Form
    {
        EventWaitHandle EventPulsar;


        public FormEventServer()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();

        }



        void SetSignal(object o)
        {
            EventWaitHandle Pulsar = new AutoResetEvent(false);
            while (true)
            {
                if (!Pulsar.WaitOne(500))          // Таймер
                {
                    EventPulsar.Set();
                }
            }
        }

        private void FormEventServer_Shown(object sender, EventArgs e)
        {
            EventPulsar = new EventWaitHandle(false, EventResetMode.AutoReset, "Bum");

            ThreadPool.QueueUserWorkItem(SetSignal);
        }
    }
}
