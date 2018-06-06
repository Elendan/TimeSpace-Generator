using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Handlers;

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

            foreach (Type handlerType in handlerTypes)
            {
                var handler = (IPacketHandler)Activator.CreateInstance(handlerType);

                foreach (MethodInfo methodInfo in handlerType.GetMethods().Where(s =>
                    s.GetCustomAttributes(false).OfType<PacketAttribute>().Any() || s.GetParameters().FirstOrDefault()?.ParameterType.BaseType == typeof(PacketDefinition)))
                {
                    List<PacketAttribute> packetAttributes = methodInfo.GetCustomAttributes(false).OfType<PacketAttribute>().ToList();

                    if (!packetAttributes.Any())
                    {
                        var methodReference = new HandlerMethodReference(
                            DelegateBuilder.BuildDelegate<Action<object, object>>(methodInfo), handler,
                            methodInfo.GetParameters().FirstOrDefault()?.ParameterType);
                        HandlerMethods.Add(methodReference.Identification, methodReference);
                    }
                    else
                    {
                        // assume string based handler method
                        foreach (PacketAttribute packetAttribute in packetAttributes)
                        {
                            var methodReference = new HandlerMethodReference(
                                DelegateBuilder.BuildDelegate<Action<object, object>>(methodInfo), handler,
                                packetAttribute);
                            HandlerMethods.Add(methodReference.Identification, methodReference);
                        }
                    }
                }
            }
        }


        public void TriggerHandlerPacket(string packetHeader, string packet, bool force = false)
        {
            //Ik, it sucks
            GenerateHandlerReferences(typeof(ScriptedInstancePacketHandler));

            if (!HandlerMethods.TryGetValue(packetHeader, out HandlerMethodReference methodReference))
            {
                MessageBox.Show($"Handler not found for packet : {packetHeader}");
                return;
            }

            try
            {
                if (methodReference.PacketDefinitionParameterType != null)
                {
                    object deserializedPacket = PacketFactory.Deserialize(packet, methodReference.PacketDefinitionParameterType, true);

                    if (deserializedPacket != null || methodReference.PassNonParseablePacket)
                    {
                        methodReference.HandlerMethod(methodReference.ParentHandler, deserializedPacket);
                    }
                    else
                    {
                        MessageBox.Show("Packet is corrupted.");
                    }
                }
                else
                {
                    methodReference.HandlerMethod(methodReference.ParentHandler, packet);
                }
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
    }
}
