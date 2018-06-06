using System.Windows.Forms;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Packets;

namespace TimeSpaceGenerator.Handlers
{
    internal class ScriptedInstancePacketHandler : IPacketHandler
    {
        public void InButtonPacket(InPacket packet)
        {
            MessageBox.Show($"entityType : {packet.EntityType.ToString()}\nNpcMonsterVnum {packet.NpcMonsterVnum}");
        }
    }
}
