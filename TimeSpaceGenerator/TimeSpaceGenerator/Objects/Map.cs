using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Map
    {
        public Map(short vnum, byte id, short indexX, short indexY)
        {
            MapButtons = new HashSet<Button>();
            MapPortals = new HashSet<Portal>();
            MapNpcs = new HashSet<Npc>();
            MapMonsters = new HashSet<Monster>();
            OnCharacterDiscoveringMap = new HashSet<Event>();
            OnMapClear = new HashSet<Event>();
            Waves = new HashSet<Wave>();

            Id = id;
            Vnum = vnum;
            IndexX = indexX;
            IndexY = indexY;
        }

        public byte Id { get; set; }

        public short Vnum { get; set; }

        public short IndexX { get; set; }

        public short IndexY { get; set; }

        public Clock Clock { get; set; }

        public Clock MapClock { get; set; }

        public HashSet<Portal> MapPortals { get; set; }

        public HashSet<Npc> MapNpcs { get; set; }

        public HashSet<Monster> MapMonsters { get; set; }

        public HashSet<Button> MapButtons { get; set; }

        public HashSet<Event> OnCharacterDiscoveringMap { get; set; }

        public HashSet<Event> OnMapClear { get; set; }

        public HashSet<Wave> Waves { get; set; }
    }
}