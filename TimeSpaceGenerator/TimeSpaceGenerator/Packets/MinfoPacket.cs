using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("minfo")]
    public class MinfoPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public string Unknown1 { get; set; }

        [PacketIndex(1)]
        public string Unknown2 { get; set; }

        [PacketIndex(2)]
        public string Unknown3 { get; set; }

        [PacketIndex(3)]
        public string Unknown4 { get; set; }

        [PacketIndex(4)]
        public string Unknown5 { get; set; }

        [PacketIndex(5)]
        public string Unknown6 { get; set; }

        [PacketIndex(6)]
        public string Unknown7 { get; set; }

        [PacketIndex(7)]
        public short Unknown8 { get; set; }

        [PacketIndex(8)]
        public string Unknown9 { get; set; }

        [PacketIndex(9)]
        public string Unknown10 { get; set; }

        [PacketIndex(10)]
        public byte Lives { get; set; }

        [PacketIndex(11)]
        public string Unknown11 { get; set; }

        [PacketIndex(12)]
        public string Unknown12 { get; set; }
    }
}
