using System.Collections.Generic;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    public class Clock : PacketDefinition
    {
        public Clock(ClockType type, int time)
        {
            TimeOutEvent = new HashSet<Event>();
            Type = type;
            Time = time;
        }

        public ClockType Type { get; set; }

        public int Time { get; set; }

        public HashSet<Event> TimeOutEvent { get; set; }
    }
}