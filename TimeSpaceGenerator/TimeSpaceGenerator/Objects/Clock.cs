using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    public class Clock
    {
        public Clock() => TimeOutEvent = new List<Event>();

        public Clock(ClockType type, int time)
        {
            TimeOutEvent = new List<Event>();
            Type = type;
            Time = time;
        }

        public ClockType Type { get; set; }

        public int Time { get; set; }

        public List<Event> TimeOutEvent { get; set; }
    }
}
