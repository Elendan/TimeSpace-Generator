using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Portal
    {
        public Portal(short posX, short posY, byte type, int id, short mapId)
        {
            OnTraversalEvent = new List<Event>();
            PosX = posX;
            PosY = posY;
            PortalType = type;
            PortalId = id;
            MapId = mapId;
        }

        public short MapId { get; set; }

        public short PosX { get; set; }

        public short PosY { get; set; }

        public byte PortalType { get; set; }

        public int PortalId { get; set; }

        public List<Event> OnTraversalEvent { get; set; }

        public short DestX { get; set; }

        public short DestY { get; set; }

        public int DestMapId { get; set; }
    }
}