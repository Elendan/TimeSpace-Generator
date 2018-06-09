using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("rsfm")]
    public class RsfmPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public short Unknown1 { get; set; }

        [PacketIndex(1)]
        public short Unknown2 { get; set; }

        [PacketIndex(2)]
        public short Unknown3 { get; set; }

        [PacketIndex(3)]
        public short Unknown4 { get; set; }
    }
}