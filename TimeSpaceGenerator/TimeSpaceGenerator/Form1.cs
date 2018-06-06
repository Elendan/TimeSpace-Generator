using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TimeSpaceGenerator.Core;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Errors;
using TimeSpaceGenerator.Handlers;

namespace TimeSpaceGenerator
{
    public partial class Form1 : Form
    {
        public TriggerHandler PacketTriggerHandler { get; set; }

        public Form1()
        {
            InitializeComponent();
            PacketTriggerHandler = new TriggerHandler();
            PacketTriggerHandler.GenerateHandlerReferences(typeof(ScriptedInstancePacketHandler));
        }

        private void GenerateXmlButton_Click(object sender, EventArgs e)
        {
            ErrorTextBox.Text = string.Empty;
            ErrorManager.Instance.Error.Clear();
            foreach (string line in PacketTextBox.Lines.Where(s => !string.IsNullOrEmpty(s)))
            {
                string[] packetSplit = line.Split(' ');
                PacketTriggerHandler.TriggerHandlerPacket(packetSplit[0], line, true);
            }

            foreach (KeyValuePair<ErrorType, string> item in ErrorManager.Instance.Error)
            {
                ErrorTextBox.Text += $"{item.Key.ToString()}: {item.Value}" + Environment.NewLine;
            }
        }
    }
}
