using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("infoi")]
    public class InfoiPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public int MessageIndex { get; set; }
    }
}
