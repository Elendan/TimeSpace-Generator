using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Objects
{
    public class Item
    {
        public Item(short vnum, short amount)
        {
            Amount = amount;
            Vnum = vnum;
        }

        public short Vnum { get; set; }

        public short Amount { get; set; }
    }
}
