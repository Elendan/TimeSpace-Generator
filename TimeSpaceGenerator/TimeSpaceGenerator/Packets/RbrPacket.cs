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
        public string Unknown1 { get; set; }

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
        public short DrawGift1 { get; set; }

        [PacketIndex(9)]
        public short DrawGiftAmount1 { get; set; }

        [PacketIndex(10)]
        public short DrawGift2 { get; set; }

        [PacketIndex(11)]
        public short DrawGiftAmount2 { get; set; }

        [PacketIndex(12)]
        public short DrawGift3 { get; set; }

        [PacketIndex(13)]
        public short DrawGiftAmount3 { get; set; }

        [PacketIndex(14)]
        public short DrawGift4 { get; set; }

        [PacketIndex(15)]
        public short DrawGiftAmount4 { get; set; }

        [PacketIndex(16)]
        public short DrawGift5 { get; set; }

        [PacketIndex(17)]
        public short DrawGiftAmount5 { get; set; }

        [PacketIndex(18)]
        public short SpecialItem1 { get; set; }

        [PacketIndex(19)]
        public short SpecialItemAmount1 { get; set; }

        [PacketIndex(20)]
        public short SpecialItem2 { get; set; }

        [PacketIndex(21)]
        public short SpecialItemAmount2 { get; set; }

        [PacketIndex(22)]
        public short BonusItem1 { get; set; }

        [PacketIndex(23)]
        public short BonusItemAmount1 { get; set; }

        [PacketIndex(24)]
        public short BonusItem2 { get; set; }

        [PacketIndex(25)]
        public short BonusItemAmount2 { get; set; }

        [PacketIndex(26)]
        public short BonusItem3 { get; set; }

        [PacketIndex(27)]
        public short BonusItemAmount3 { get; set; }

        [PacketIndex(28)]
        public int WinnerScore { get; set; }

        [PacketIndex(29)]
        public string WinnerName { get; set; }

        [PacketIndex(30)]
        public int Unknown6 { get; set; }

        [PacketIndex(31)]
        public int Unknown7 { get; set; }

        [PacketIndex(32, SerializeToEnd = true)]
        public string TitleAndLabel { get; set; }
    }
}
