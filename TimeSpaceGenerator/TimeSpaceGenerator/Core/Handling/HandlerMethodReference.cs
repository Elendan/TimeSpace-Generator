using System;
using System.Linq;
using TimeSpaceGenerator.Core.Serializing;

namespace TimeSpaceGenerator.Core.Handling
{
    public class HandlerMethodReference
    {
        #region Instantiation

        public HandlerMethodReference(Action<object, object> handlerMethod, IPacketHandler parentHandler, PacketAttribute handlerMethodAttribute)
        {
            HandlerMethod = handlerMethod;
            ParentHandler = parentHandler;
            HandlerMethodAttribute = handlerMethodAttribute;
            Identification = HandlerMethodAttribute.Header;
            PassNonParseablePacket = false;
        }

        public HandlerMethodReference(Action<object, object> handlerMethod, IPacketHandler parentHandler, Type packetBaseParameterType)
        {
            HandlerMethod = handlerMethod;
            ParentHandler = parentHandler;
            PacketDefinitionParameterType = packetBaseParameterType;
            var headerAttribute = (PacketHeaderAttribute)PacketDefinitionParameterType.GetCustomAttributes(true).FirstOrDefault(ca => ca.GetType() == typeof(PacketHeaderAttribute));
            Identification = headerAttribute?.Identification;
            PassNonParseablePacket = headerAttribute?.PassNonParseablePacket ?? false;
        }

        #endregion

        #region Properties

        public Action<object, object> HandlerMethod { get; }

        public PacketAttribute HandlerMethodAttribute { get; }

        /// <summary>
        ///     Unique identification of the Packet by Header
        /// </summary>
        public string Identification { get; }

        public Type PacketDefinitionParameterType { get; }

        public IPacketHandler ParentHandler { get; }

        public bool PassNonParseablePacket { get; }

        #endregion
    }
}