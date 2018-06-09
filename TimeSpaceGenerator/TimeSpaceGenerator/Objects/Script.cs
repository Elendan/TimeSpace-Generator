using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Script
    {
        public Script()
        {
            Maps = new List<Map>();
            Info = new TimeSpaceInfo();
        }

        public List<Map> Maps { get; set; }

        public TimeSpaceInfo Info { get; set; }
    }
}