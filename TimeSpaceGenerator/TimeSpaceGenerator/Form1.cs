using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeSpaceGenerator.Core;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Errors;
using TimeSpaceGenerator.Handlers;
using TimeSpaceGenerator.Helpers;
using TimeSpaceGenerator.Managers;

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
            ScriptManager.Instance.RbrPacketManager(RbrPacketTextBox.Text, XmlFileNameTextBox);
            foreach (string line in PacketTextBox.Lines.Where(s => !string.IsNullOrEmpty(s)))
            {
                string cpy = line;
                if (!char.IsLetter(line[0]))
                {
                    //I know this is dirty as fuck
                    cpy = line.Remove(0, PacketHelper.Instance.RemovableStringIndex(line, '[', ']', 2, 2));
                }
                string[] packetSplit = cpy.Split(' ');
                cpy = PacketHelper.Instance.FormatPacket(cpy, ' ');
                PacketTriggerHandler.TriggerHandlerPacket(packetSplit[0], cpy, true);
            }
            
            foreach (KeyValuePair<ErrorType, string> item in ErrorManager.Instance.Error)
            {
                ErrorTextBox.Text += $"{item.Key.ToString()}: {item.Value}" + Environment.NewLine;
            }

            if (!ScriptManager.Instance.IsGenerated)
            {
                //Dont ask me why I did such bullshit, will fix.
                ScriptManager.Instance.ScriptData = ScriptManager.Instance.GenerateScript(ScriptManager.Instance.Script);
                XmlOutputBox.Text = ScriptManager.Instance.GenerateScript(ScriptManager.Instance.Script);
                ScriptManager.Instance.IsGenerated = true;
            }

            XmlOutputBox.Text += ScriptManager.Instance.ScriptData;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PacketsLabel_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GenerateXmlFileButton_Click(object sender, EventArgs e)
        {
            string path = string.Format("Scripts");
            string str = string.Format("{0}\\{1}", (object)path, (object)XmlFileNameTextBox.Text);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.Create(string.Format("{0}", (object)str)).Close();
            File.WriteAllText(string.Format("{0}", (object)str), ScriptManager.Instance.ScriptData, Encoding.Unicode);
            MessageBox.Show("File sucessfuly created", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
