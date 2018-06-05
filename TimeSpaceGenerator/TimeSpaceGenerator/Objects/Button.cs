using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Objects
{
    public class Button
    {
        public Button(short vnum, long buttonId, short posX, short posY)
        {
            OnFirstEnable = new List<Event>();
            OnEnable = new List<Event>();

            ButtonId = buttonId;
            PosX = posX;
            PosY = posY;
            DisableVnum = vnum;
            switch (vnum)
            {
                case 1000:
                    EnableVnum = 1045;
                    break;
                case 1057:
                    EnableVnum = 1057;
                    break;
                default:
                    EnableVnum = (short)(vnum + 1);
                    break;
            }
        }

        public long ButtonId { get; set; }

        public short PosX { get; set; }

        public short PosY { get; set; }

        public short EnableVnum { get; set; }

        public short DisableVnum { get; set; }

        public List<Event> OnFirstEnable { get; set; }

        public List<Event> OnEnable { get; set; }
    }
}
