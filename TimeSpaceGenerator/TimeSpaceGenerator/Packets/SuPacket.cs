using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("su")]
    public class SuPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public VisualType EntityType { get; set; }

        [PacketIndex(1)]
        public long TargetId { get; set; }

        [PacketIndex(2)]
        public short Unknown1 { get; set; }

        [PacketIndex(3)]
        public long EntityId { get; set; }

        [PacketIndex(4)]
        public short Unknown2 { get; set; }

        [PacketIndex(5)]
        public short Unknown3 { get; set; }

        [PacketIndex(6)]
        public short Unknown4 { get; set; }

        [PacketIndex(7)]
        public short SkillEffect { get; set; }

        [PacketIndex(8)]
        public short Unknown5 { get; set; }

        [PacketIndex(9)]
        public short Unknown6 { get; set; }

        [PacketIndex(10)]
        public short Unknown7 { get; set; }

        [PacketIndex(11)]
        public int Hp { get; set; }

        [PacketIndex(12)]
        public int Damage { get; set; }

        [PacketIndex(13)]
        public short Unknown8 { get; set; }

        [PacketIndex(14)]
        public short Unknown9 { get; set; }
    }
}