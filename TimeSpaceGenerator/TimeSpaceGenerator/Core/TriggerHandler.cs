using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSpaceGenerator.Core.Handling;

namespace TimeSpaceGenerator.Core
{
    public class TriggerHandler
    {
        #region Instantiation

        public TriggerHandler()
        {
            HandlerMethods = new Dictionary<string, HandlerMethodReference>();
        }

        #endregion

        #region Properties

        public Dictionary<string, HandlerMethodReference> HandlerMethods { get; set; }

        #endregion

        #region Methods

        private void GenerateHandlerReferences(Type type)
        {
            IEnumerable<Type> handlerTypes = type.Assembly.GetTypes().Where(s => s.Name.Equals("ScriptedInstancePacketHandler"));
        }


        public void TriggerHandlerPacket(string packetHeader, string packet, bool force)
        {

        }

        #endregion
    }
}
