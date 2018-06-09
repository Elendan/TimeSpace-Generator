using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("in")]
    public class InPacket : PacketDefinition
    {
        #region Properties
        [PacketIndex(0)]
        public VisualType EntityType { get; set; }

        [PacketIndex(1)]
        public short NpcMonsterVnum { get; set; }

        [PacketIndex(2)]
        public long MateTransportId { get; set; }

        [PacketIndex(3)]
        public short PosX { get; set; }

        [PacketIndex(4)]
        public short PosY { get; set; }

        [PacketIndex(5)]
        public short Direction { get; set; }

        [PacketIndex(6)]
        public int Hp { get; set; }

        [PacketIndex(7)]
        public int Mp { get; set; }

        [PacketIndex(8)]
        public int Unknown { get; set; }

        [PacketIndex(9)]
        public short Faction { get; set; }

        [PacketIndex(10)]
        public int Unknown2 { get; set; }

        [PacketIndex(11)]
        public long CharacterId { get; set; }

        [PacketIndex(12)]
        public int Unknown3 { get; set; }

        [PacketIndex(13)]
        public int Unknown4 { get; set; }

        [PacketIndex(14)]
        public short Morph { get; set; }

        [PacketIndex(15)]
        public string Name { get; set; }

        [PacketIndex(16)]
        public short MateType { get; set; }

        [PacketIndex(17)]
        public int Unkown5 { get; set; }

        [PacketIndex(18)]
        public int Unkown6 { get; set; }

        [PacketIndex(19)]
        public int Unkown7 { get; set; }

        [PacketIndex(20)]
        public int Unkown8 { get; set; }

        [PacketIndex(21)]
        public int Unkown9 { get; set; }

        [PacketIndex(22)]
        public int Unkown10 { get; set; }

        [PacketIndex(23)]
        public int Unkown11 { get; set; }

        [PacketIndex(24)]
        public int Unkown12 { get; set; }

        [PacketIndex(25)]
        public int Unkown13 { get; set; }
        #endregion
    }
}
