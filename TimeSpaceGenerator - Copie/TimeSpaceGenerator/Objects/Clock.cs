using System.Collections.Generic;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Objects
{
    public class Clock : PacketDefinition
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
