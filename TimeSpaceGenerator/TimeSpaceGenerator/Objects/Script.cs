using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
