using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TimeSpaceGenerator.Core;
using TimeSpaceGenerator.Errors;
using TimeSpaceGenerator.Handlers;
using TimeSpaceGenerator.Helpers;
using TimeSpaceGenerator.Managers;
using TimeSpaceGenerator.Objects;
using XmlFilePreview;

namespace TimeSpaceGenerator
{
    public partial class MainWindow : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        private readonly TriggerHandler _packetTriggerHandler;

        public string XmlFile;

        public MainWindow()
        {
            InitializeComponent();
            _packetTriggerHandler = new TriggerHandler();
            _packetTriggerHandler.GenerateHandlerReferences(typeof(ScriptedInstancePacketHandler));
        }


        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void GenerateXmlButton_Click(object sender, EventArgs e)
        {
            try
            {
                ErrorTextBox.Text = string.Empty;
                ErrorManager.Instance.Error.Clear();
                ScriptManager.Instance.IsGenerated = false;
                ScriptManager.Instance.LabelSet = false;
                ScriptManager.Instance.Script = new Script();
                ScriptManager.Instance.ScriptData = string.Empty;
                foreach (string line in PacketTextBox.Lines.Where(s => !string.IsNullOrEmpty(s)))
                {
                    string cpy = line;
                    if (!char.IsLetter(line[0]) || !char.IsDigit(line[0]))
                    {
                        //I know this is dirty as fuck
                        cpy = line.Remove(0, PacketHelper.Instance.RemovableStringIndex(line, '[', ']', 2, 2));
                    }

                    string[] packetSplit = cpy.Split(' ');
                    cpy = PacketHelper.Instance.FormatPacket(cpy, ' ');
                    _packetTriggerHandler.TriggerHandlerPacket(packetSplit[0], cpy, true);
                }

                XmlFileNameTextBox.Text = ScriptManager.Instance.FileName;

                ErrorManager.Instance.Dump(ErrorTextBox);

                if (!ScriptManager.Instance.IsGenerated)
                {
                    //Dont ask me why I did such bullshit, will fix.
                    ScriptManager.Instance.ScriptData = ScriptManager.Instance.GenerateScript();
                    ScriptManager.Instance.IsGenerated = true;
                }

                XmlFile = ScriptManager.Instance.ScriptData;
            }
            catch (Exception)
            {
                MessageBox.Show("Please, use correctly formatted packets\nExamples of good formated packets:\n\n[21:12:22] [RECV] Out 1 1\nOut 1 1");
            }
            
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
            const string path = "Scripts";
            string str = $"{path}\\{XmlFileNameTextBox.Text}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.Create($"{str}").Close();
            File.WriteAllText($"{str}", ScriptManager.Instance.ScriptData, Encoding.Unicode);
            MessageBox.Show(@"File sucessfuly created", @"Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void ButtonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonXmlFilePreview_Click(object sender, EventArgs e)
        {
            var x = new XmlFilePreviewWindow(XmlFile);
            x.ShowDialog();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
    }
}