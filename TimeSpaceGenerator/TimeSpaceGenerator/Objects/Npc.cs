namespace TimeSpaceGenerator.Objects
{
    public class Npc
    {
        public Npc(long vnum, long npcId, long posX, long posY)
        {
            Vnum = vnum;
            MapNpcId = npcId;
            PosX = posX;
            PosY = posY;
        }

        public long MapNpcId { get; set; }

        public long PosX { get; set; }

        public long PosY { get; set; }

        public long Vnum { get; set; }

        public bool IsMate { get; set; }

        public bool IsProtected { get; set; }
    }
}