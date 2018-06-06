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
using TimeSpaceGenerator.Core;

namespace TimeSpaceGenerator
{
    public partial class Form1 : Form
    {
        public TriggerHandler PacketTriggerHandler { get; set; }

        public Form1()
        {
            InitializeComponent();
            PacketTriggerHandler = new TriggerHandler();
        }

        private void GenerateXmlButton_Click(object sender, EventArgs e)
        {
            foreach (string line in PacketTextBox.Lines)
            {
                string[] packetSplit = line.Split(' ');
                PacketTriggerHandler.TriggerHandlerPacket(packetSplit[0], line, false);
            }
        }
    }
}
