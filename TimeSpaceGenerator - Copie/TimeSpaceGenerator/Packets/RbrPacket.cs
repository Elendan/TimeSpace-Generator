using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Packets
{
    [PacketHeader("rbr")]
    public class RbrPacket : PacketDefinition
    {
        [PacketIndex(0)]
        public int Unknown1 { get; set; }

        [PacketIndex(1)]
        public int Unknown2 { get; set; }

        [PacketIndex(2)]
        public int Unknown3 { get; set; }

        [PacketIndex(3)]
        public int Unknown4 { get; set; }

        [PacketIndex(4)]
        public int Unknown5 { get; set; }

        [PacketIndex(5)]
        public byte LevelMinimum { get; set; }

        [PacketIndex(6)]
        public byte LevelMaximum { get; set; }

        [PacketIndex(7)]
        public int GiftSum { get; set; }

        [PacketIndex(8)]
        public string DrawGift { get; set; }

        [PacketIndex(9)]
        public string SpecialItems { get; set; }

        [PacketIndex(10)]
        public string BonusItems { get; set; }

        [PacketIndex(11)]
        public int WinnerScore { get; set; }

        [PacketIndex(12)]
        public string WinnerName { get; set; }

        [PacketIndex(13)]
        public int Unknown6 { get; set; }

        [PacketIndex(14)]
        public int Unknown7 { get; set; }

        [PacketIndex(15)]
        public string TitleAndLabel { get; set; }
    }
}
