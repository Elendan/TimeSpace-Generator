using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Helpers;
using TimeSpaceGenerator.Objects;
using Message = System.Windows.Forms.Message;

namespace TimeSpaceGenerator.Handlers
{
    internal class ScriptedInstancePacketHandler : IPacketHandler
    {
        public void InButtonPacket(InPacket packet)
        {
            MessageBox.Show($"entityType : {packet.EntityType}\nNpcMonsterVnum {packet.NpcMonsterVnum}");
        }
    }
}
