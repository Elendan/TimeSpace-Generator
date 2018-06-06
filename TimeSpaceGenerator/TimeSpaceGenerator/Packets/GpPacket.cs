using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("gp")]
    public class GpPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public short SourceX { get; set; }

        [PacketIndex(1)]
        public short SourceY { get; set; }

        [PacketIndex(2)]
        public short MapId { get; set; }

        [PacketIndex(3)]
        public short PortalType { get; set; }

        [PacketIndex(4)]
        public int PortalId { get; set; }

        [PacketIndex(5)]
        public bool IsDisabled { get; set; }
    }
}
