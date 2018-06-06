using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Core.Serializing
{
    public abstract class PacketDefinition
    {
        public string OriginalContent { get; set; }

        public string OriginalHeader { get; set; }
    }
}
