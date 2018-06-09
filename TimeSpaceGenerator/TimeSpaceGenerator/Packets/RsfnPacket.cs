using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("rsfn")]
    public class RsfnPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public short MapIndexX { get; set; }

        [PacketIndex(1)]
        public short MapIndexY { get; set; }

        [PacketIndex(2)]
        public bool EnnemiesStillAlive { get; set; }
    }
}