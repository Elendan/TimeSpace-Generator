using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Objects;

namespace TimeSpaceGenerator.Handlers
{
    internal class ScriptedInstancePacketHandler : IPacketHandler
    {
        public void InButtonPacket(ButtonPacket packet)
        {
            MessageBox.Show("In packet was found !");
        }
    }
}
