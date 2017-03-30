using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
namespace EventLanch
{
    public partial class FormEventLanch : Form
    {
        EventWaitHandle EventPulsar;

        public FormEventLanch()
        {
            InitializeComponent();
        }

        void WaitSignal(object o)
        {
            while (true)
            {
                if (EventPulsar.WaitOne(3000))
                {
                    textBox1.Invoke(
                    new Action(delegate
                    {
                        textBox1.AppendText("Пришол сигнал" + Environment.NewLine);
                    })
                    );

                }
                else
                {
                    textBox1.Invoke(
                    new Action(delegate
                    {
                        textBox1.AppendText("Прошло 3 секунды недождались" + Environment.NewLine);
                    })
                    );

                }
            }
        }

        private void FormEventLanch_Shown(object sender, EventArgs e)
        {
            bool creat=false;
            EventPulsar = new EventWaitHandle(false, EventResetMode.AutoReset, "Bum",out creat);
            textBox1.AppendText(creat.ToString() + Environment.NewLine);

            // false - программа уже запущена
            // true  - программа не запущена надо запкустить

            ThreadPool.QueueUserWorkItem(WaitSignal);
        }
    }
}
