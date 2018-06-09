using System.Collections.Generic;

namespace TimeSpaceGenerator.Objects
{
    public class Script
    {
        public Script()
        {
            this.Maps = new List<Map>();
            this.Info = new TimeSpaceInfo();
        }

        public List<Map> Maps { get; set; }

        public TimeSpaceInfo Info { get; set; }
    }
}
