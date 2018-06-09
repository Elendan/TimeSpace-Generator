using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class TimeSpaceInfo
    {
        public TimeSpaceInfo()
        {
            DrawGift = new List<Item>();
            Special = new List<Item>();
            Bonus = new List<Item>();
        }

        public List<Item> Bonus { get; set; }

        public List<Item> DrawGift { get; set; }

        public string Title { get; set; }

        public string Label { get; set; }

        public byte LevelMax { get; set; }

        public byte LevelMinimum { get; set; }

        public byte Lives { get; set; }

        public List<Item> Special { get; set; }

        public short SumOfRequired { get; set; }
    }
}