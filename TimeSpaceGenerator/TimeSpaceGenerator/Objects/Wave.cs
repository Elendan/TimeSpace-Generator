﻿using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Wave
    {
        public Wave() => Events = new List<Event>();

        public short OffSet { get; set; }

        public int Delay { get; set; }

        public List<Event> Events { get; set; }
    }
}