﻿using System;

namespace TimeSpaceGenerator.Core.Serializing
{
    public class PacketHeaderAttribute : Attribute
    {
        #region Instantiation

        public PacketHeaderAttribute(string identification) => Identification = identification;

        #endregion

        #region Properties

        /// <summary>
        ///     Unique identification of the Packet
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        ///     Pass the packet to handler method even if the serialization has failed.
        /// </summary>
        public bool PassNonParseablePacket { get; set; }

        #endregion
    }
}