using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Button
    {
        public Button(long vnum, long buttonId, long posX, long posY)
        {
            OnFirstEnable = new List<Event>();
            ButtonId = buttonId;
            PosX = posX;
            PosY = posY;
            DisableVNum = vnum;
            switch (vnum)
            {
                case 1000:
                    EnableVNum = 1045;
                    break;
                case 1057:
                    EnableVNum = 1057;
                    break;
                default:
                    EnableVNum = (short)(vnum + 1);
                    break;
            }
        }

        public long ButtonId { get; set; }

        public long PosX { get; set; }

        public long PosY { get; set; }

        public long EnableVNum { get; set; }

        public long DisableVNum { get; set; }

        public List<Event> OnFirstEnable { get; set; }
    }
}