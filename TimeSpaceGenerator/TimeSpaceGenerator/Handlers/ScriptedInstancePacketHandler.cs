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

        public void RbrPacket(RbrPacket packet)
        {
            MessageBox.Show($"{packet.LevelMinimum}\n{packet.Unknown1}\n{packet.Unknown2}\n{packet.Unknown3}\n{packet.Unknown4}\n{packet.Unknown5}\n");
        }
    }
}
