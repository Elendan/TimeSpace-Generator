using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("npc_req")]
    public class NpcReqPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public byte RequestType { get; set; }

        [PacketIndex(1)]
        public long MapNpcId { get; set; }

        [PacketIndex(2)]
        public short Dialog { get; set; }
    }
}
