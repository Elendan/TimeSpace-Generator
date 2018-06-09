using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("out")]
    public class OutPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public VisualType Type { get; set; }

        [PacketIndex(1)]
        public long EntityId { get; set; }
    }
}