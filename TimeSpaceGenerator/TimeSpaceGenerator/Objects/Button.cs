using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    public class Button
    {
        public Button(short vnum, long buttonId, short posX, short posY)
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

        public short PosX { get; set; }

        public short PosY { get; set; }

        public short EnableVNum { get; set; }

        public short DisableVNum { get; set; }

        public List<Event> OnFirstEnable { get; set; }
    }
}
