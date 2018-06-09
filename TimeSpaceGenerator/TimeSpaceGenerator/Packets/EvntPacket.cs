using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Packets
{
    // Basically events such as Clocks
    [PacketHeader("evnt")]
    public class EvntPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public ClockType Type { get; set; }

        [PacketIndex(1)]
        public int Enabled { get; set; } //Idk really

        [PacketIndex(2)]
        public int DeciSecondsRemaining { get; set; }

        [PacketIndex(3)]
        public int BaseSecondsRemaining { get; set; }
    }
}