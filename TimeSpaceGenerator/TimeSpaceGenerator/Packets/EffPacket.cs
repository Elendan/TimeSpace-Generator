using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("eff")]
    public class EffPacket : PacketDefinition
    {
        #region Properties

        [PacketIndex(0)]
        public byte EffectType { get; set; }

        [PacketIndex(1)]
        public long EntityId { get; set; }

        [PacketIndex(2)]
        public int Id { get; set; }

        #endregion
    }
}