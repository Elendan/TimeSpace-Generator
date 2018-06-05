using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Objects
{
    public class Npc
    {
        public Npc(short vnum, long npcId, short posX, short posY)
        {
            Vnum = vnum;
            MapNpcId = npcId;
            PosX = posX;
            PosY = posY;
        }

        public long MapNpcId { get; set; }

        public short PosX { get; set; }

        public short PosY { get; set; }

        public short Vnum { get; set; }

        public bool IsMate { get; set; }

        public bool IsProtected { get; set; }
    }
}
