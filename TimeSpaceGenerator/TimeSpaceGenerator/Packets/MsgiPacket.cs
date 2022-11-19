using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("msgi")]
    public class MsgiPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public byte Type { get; set; }

        [PacketIndex(1)]
        public int MessageIndex { get; set; }
    }
}
