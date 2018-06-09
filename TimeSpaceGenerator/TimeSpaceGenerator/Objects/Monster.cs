using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Monster
    {
        public Monster(short vnum, long mapMonsterId, short posX, short posY)
        {
            OnDeathEvents = new List<Event>();

            MapMonsterId = mapMonsterId;
            PosX = posX;
            PosY = posY;
            Vnum = vnum;
        }

        public long MapMonsterId { get; set; }

        public short PosX { get; set; }

        public short PosY { get; set; }

        public short Vnum { get; set; }

        public bool IsTarget { get; set; }

        public bool IsBonus { get; set; }

        public bool IsDead { get; set; }

        public List<Event> OnDeathEvents { get; set; }
    }
}