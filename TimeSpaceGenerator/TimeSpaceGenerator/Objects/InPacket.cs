using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    [PacketHeader("in")]
    public class InPacket : PacketDefinition
    {
        #region Properties
        [PacketIndex(0)]
        public int EntityType { get; set; }

        [PacketIndex(1)]
        public long NpcMonsterVnum { get; set; }

        [PacketIndex(2)]
        public long MateTransportId { get; set; }

        [PacketIndex(3)]
        public short PosX { get; set; }

        [PacketIndex(4)]
        public short PosY { get; set; }

        [PacketIndex(5)]
        public sbyte Direction { get; set; }

        [PacketIndex(6)]
        public int Hp { get; set; }

        [PacketIndex(7)]
        public int Mp { get; set; }

        [PacketIndex(8)]
        public sbyte Unknown { get; set; }

        [PacketIndex(9)]
        public sbyte Faction { get; set; }

        [PacketIndex(10)]
        public sbyte Unknown2 { get; set; }

        [PacketIndex(11)]
        public long CharacterId { get; set; }

        [PacketIndex(12)]
        public sbyte Unknown3 { get; set; }

        [PacketIndex(13)]
        public sbyte Unknown4 { get; set; }

        [PacketIndex(14)]
        public short Morph { get; set; }

        [PacketIndex(15)]
        public string Name { get; set; }

        [PacketIndex(16)]
        public sbyte MateType { get; set; }

        [PacketIndex(17)]
        public sbyte Unkown5 { get; set; }

        [PacketIndex(18)]
        public sbyte Unkown6 { get; set; }

        [PacketIndex(19)]
        public sbyte Unkown7 { get; set; }

        [PacketIndex(20)]
        public sbyte Unkown8 { get; set; }

        [PacketIndex(21)]
        public sbyte Unkown9 { get; set; }

        [PacketIndex(22)]
        public sbyte Unkown10 { get; set; }

        [PacketIndex(23)]
        public sbyte Unkown11 { get; set; }

        [PacketIndex(24)]
        public sbyte Unkown12 { get; set; }

        [PacketIndex(25)]
        public sbyte Unkown13 { get; set; }
        #endregion
    }
}
