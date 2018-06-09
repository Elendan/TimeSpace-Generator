using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlFilePreview
{
    public partial class XmlFilePreviewWindow : Form
    {
        public XmlFilePreviewWindow(string XmlFile)
        {
            InitializeComponent();
            XmlOutputBox.Text = XmlFile;
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
