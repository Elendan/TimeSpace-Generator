using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TimeSpaceGenerator.Core.Handling;
using TimeSpaceGenerator.Core.Serializing;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Errors;
using TimeSpaceGenerator.Managers;

namespace TimeSpaceGenerator.Core
{
    public class TriggerHandler
    {
        #region Members

        private IDictionary<string, HandlerMethodReference> _handlerMethods;

        #endregion

        #region Instantiation

        public TriggerHandler() => HandlerMethods = new Dictionary<string, HandlerMethodReference>();

        #endregion

        #region Properties

        public IDictionary<string, HandlerMethodReference> HandlerMethods
        {
            get => _handlerMethods ?? (_handlerMethods = new Dictionary<string, HandlerMethodReference>());

            set => _handlerMethods = value;
        }

        #endregion

        #region Methods

        public void GenerateHandlerReferences(Type type)
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

        string LastPacketHeader = "";
        public void TriggerHandlerPacket(string packetHeader, string packet, bool force = false)
        {
            if (!HandlerMethods.TryGetValue(packetHeader, out HandlerMethodReference methodReference))
            {
                if (!ScriptManager.Instance.LabelSet)
                {
                    ScriptManager.Instance.Script.Info.Label = packet;
                    ScriptManager.Instance.LabelSet = true;
                }

                if (LastPacketHeader != "rbr")//Is ugly, but i dont want error in the box for the title
                {
                    ErrorManager.Instance.Error.Add(new KeyValuePair<ErrorType, string>(ErrorType.MissingPacket, $"Handler not found for packet : {packetHeader}"));
                    LastPacketHeader = packetHeader;
                }
                return;
            }
            LastPacketHeader = packetHeader;

            try
            {
                // call actual handler method
                if (methodReference.PacketDefinitionParameterType != null)
                {
                    object deserializedPacket = PacketFactory.Deserialize(packet,
                        methodReference.PacketDefinitionParameterType);

                    if (deserializedPacket != null || methodReference.PassNonParseablePacket)
                    {
                        methodReference.HandlerMethod(methodReference.ParentHandler, deserializedPacket);
                    }
                    else
                    {
                        ErrorManager.Instance.Error.Add(new KeyValuePair<ErrorType, string>(ErrorType.CorruptedPacket, $"Packet is corrupted"));
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