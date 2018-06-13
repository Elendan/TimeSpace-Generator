using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Monster
    {
        public Monster(long vnum, long mapMonsterId, long posX, long posY)
        {
            OnDeathEvents = new HashSet<Event>();

            MapMonsterId = mapMonsterId;
            PosX = posX;
            PosY = posY;
            Vnum = vnum;
        }

        public long MapMonsterId { get; set; }

        public long PosX { get; set; }

        public long PosY { get; set; }

        public long Vnum { get; set; }

        public bool IsTarget { get; set; }

        public bool IsBonus { get; set; }

        public bool IsDead { get; set; }

        public HashSet<Event> OnDeathEvents { get; set; }
    }
}