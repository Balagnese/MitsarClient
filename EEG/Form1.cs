using EEG.MitsarSmartBCI;
using Mitsar.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG
{
    public partial class Form1 : Form
    {
        MitsarSmartBCI.MitsarSmartBCI mitsar = null;
        MitsarSmartBCIDataControl datacontrol = null;
        MitsarSmartBCIViewControl viewControl = null;
        MitsarSmartBCIBataryControl batarycontrol = null;
        MitsarSmartBCIChanelsState chanelscontrol = null;
        Thread t;
        public Form1()
        {
            InitializeComponent();
            mitsar = new MitsarSmartBCI.MitsarSmartBCI();
            chanelscontrol = new MitsarSmartBCIChanelsState();
            chanelscontrol.EEG = mitsar;
            Controls.Add(chanelscontrol);
            datacontrol = new MitsarSmartBCIDataControl();
            batarycontrol = new MitsarSmartBCIBataryControl();
            viewControl = new MitsarSmartBCIViewControl();
            Controls.Add(datacontrol);
            Controls.Add(batarycontrol);
            Controls.Add(viewControl);
            datacontrol.EEG = mitsar;
            batarycontrol.EEG = mitsar;
            viewControl.EEG = mitsar;
            datacontrol.Location = new Point(0, 40);
            chanelscontrol.Location = new Point(150, 0);
            viewControl.Location = new Point(430, 0);
            start();
        }

        void start()
        {
            t = new Thread(startThread);
            t.Start();
        }

        void startThread()
        {
            mitsar.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stop();
        }

        void stop()
        {
            t.Abort();
            mitsar.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            start();
        }
    }
}
