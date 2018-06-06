using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Objects
{
    public class Map
    {
        public Map(short vnum, byte id, byte indexX, byte indexY)
        {
            MapButtons = new List<ButtonPacket>();
            MapPortals = new List<Portal>();
            MapNpcs = new List<Npc>();
            MapMonsters = new List<Monster>();
            OnCharacterDiscoveringMap = new List<Event>();
            OnMapClear = new List<Event>();
            Waves = new List<Wave>();

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

        public List<Portal> MapPortals { get; set; }

        public List<Npc> MapNpcs { get; set; }

        public List<Monster> MapMonsters { get; set; }

        public List<ButtonPacket> MapButtons { get; set; }

        public List<Event> OnCharacterDiscoveringMap { get; set; }

        public List<Event> OnMapClear { get; set; }

        public List<Wave> Waves { get; set; }
    }
}
