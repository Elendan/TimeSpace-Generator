using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Script
    {
        public Script()
        {
            Maps = new HashSet<Map>();
            Info = new TimeSpaceInfo();
        }

        public HashSet<Map> Maps { get; set; }

        public TimeSpaceInfo Info { get; set; }
    }
}