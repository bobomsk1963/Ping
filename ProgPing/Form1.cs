using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Net.NetworkInformation;
//using Ping;
namespace ProgPing
{
    public partial class Form1 : Form
    {
        bool BeginExit=false;

        Ping pingSender = new Ping();

        volatile bool PingWork = false;

        int TimeSleep = 500;

        string ip = "";

        public Form1()
        {
            InitializeComponent();
            
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!PingWork)
            {
                try{
                    TimeSleep = int.Parse(textBox3.Text);
                }
                catch { }

                ip = textBox1.Text;
                PingWork = true;

                try{
                    pingSender.SendAsync(ip, null);
                }
                catch (Exception eee){
                    PingWork = false;
                    textBox2.AppendText(eee.Message + Environment.NewLine);
                    return;
                }

                button1.Text = "Stop";
            }
            else { 
                PingWork = false;
                button1.Text = "Ping";
            }

            
        }

        private //static 
            void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            if (!PingWork){
                if (BeginExit) { Close(); } 
                return; 
            }

            if (e.Cancelled){
                textBox2.BeginInvoke(new Action(delegate{
                    textBox2.AppendText("e.Cancelled" + Environment.NewLine);
                }));
            }

            if (e.Error != null){
                textBox2.BeginInvoke(new Action(delegate{
                    textBox2.AppendText("e.Error" + Environment.NewLine);
                }));
            }

            PingReply reply = e.Reply;

            textBox2.Invoke
            (
            new Action(delegate
            {
                if (reply == null)
                    return;

                if (reply.Status == IPStatus.Success){
                    textBox2.AppendText("Ping Статус: " + reply.Status +
                                        " Ответ от " + reply.Address.ToString() + " число байт=" + reply.Buffer.Length +
                                        " время=" + reply.RoundtripTime + " TTL=" + reply.Options.Ttl + Environment.NewLine);    
                }
                else { textBox2.AppendText("Ping Статус: " + reply.Status + Environment.NewLine); }
            })
            );

            Thread.Sleep(TimeSleep);

            try
            {
                pingSender.SendAsync(ip, null);
            }
            catch (Exception eee)
            {
                textBox2.BeginInvoke(new Action(delegate{
                    button1.Text = "Ping"; ;
                    textBox2.AppendText(eee.Message + Environment.NewLine);
                }));
                PingWork = false;                
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PingWork)
            {
                e.Cancel = true;
                BeginExit = true;
                PingWork = false;
            }
        }
    }
}
