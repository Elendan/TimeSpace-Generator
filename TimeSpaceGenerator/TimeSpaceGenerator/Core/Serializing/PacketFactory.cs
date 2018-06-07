﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TimeSpaceGenerator.Enums;
using TimeSpaceGenerator.Errors;

namespace TimeSpaceGenerator.Core.Serializing
{
    public static class PacketFactory
    {
        #region Members

        private static Dictionary<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> _packetSerializationInformations;

        #endregion

        #region Properties

        public static bool IsInitialized { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Deserializes a string into a PacketDefinition
        /// </summary>
        /// <param name="packetContent">The content to deseralize</param>
        /// <param name="packetType">The type of the packet to deserialize to</param>
        ///     Include the keep alive identity or exclude it
        /// </param>
        /// <returns>The deserialized packet.</returns>
        public static PacketDefinition Deserialize(string packetContent, Type packetType)
        {
            try
            {
                KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> serializationInformation = GetSerializationInformation(packetType);
                var deserializedPacket = (PacketDefinition)Activator.CreateInstance(packetType); // reflection is bad, improve?
                SetDeserializationInformations(deserializedPacket, packetContent, serializationInformation.Key.Item2);
                deserializedPacket = Deserialize(packetContent, deserializedPacket, serializationInformation);
                return deserializedPacket;
            }
            catch (Exception ex)
            {
                ErrorManager.Instance.Error.Add(new KeyValuePair<ErrorType, string>(ErrorType.WrongFormat, ex.ToString()));
                ErrorManager.Instance.Error.Add(new KeyValuePair<ErrorType, string>(ErrorType.WrongFormat, $"The serialized packet has the wrong format. Packet: {packetContent}"));
                return null;
            }
        }

        private static PacketDefinition Deserialize(string packetContent, PacketDefinition deserializedPacket, KeyValuePair<Tuple<Type, string>,
            Dictionary<PacketIndexAttribute, PropertyInfo>> serializationInformation)
        {
            MatchCollection matches = Regex.Matches(packetContent, @"([^\040]+[\.][^\040]+[\040]?)+((?=\040)|$)|([^\040]+)((?=\040)|$)");

            if (matches.Count <= 0)
            {
                return deserializedPacket;
            }

            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> packetBasePropertyInfo in serializationInformation.Value)
            {
                int currentIndex = packetBasePropertyInfo.Key.Index + 1; // adding 2 because we need to skip incrementing number and packet header

                if (currentIndex < matches.Count)
                {
                    if (packetBasePropertyInfo.Key.SerializeToEnd)
                    {
                        // get the value to the end and stop deserialization
                        string valueToEnd = packetContent.Substring(matches[currentIndex].Index, packetContent.Length - matches[currentIndex].Index);
                        packetBasePropertyInfo.Value.SetValue(deserializedPacket,
                            DeserializeValue(packetBasePropertyInfo.Value.PropertyType, valueToEnd, packetBasePropertyInfo.Key, matches));
                        break;
                    }


                    string currentValue = matches[currentIndex].Value;

                    if (packetBasePropertyInfo.Value.PropertyType == typeof(string) && string.IsNullOrEmpty(currentValue))
                    {
                        throw new NullReferenceException();
                    }

                    // set the value & convert currentValue
                    packetBasePropertyInfo.Value.SetValue(deserializedPacket,
                        DeserializeValue(packetBasePropertyInfo.Value.PropertyType, currentValue, packetBasePropertyInfo.Key, matches));
                }
                else
                {
                    break;
                }
            }

            return deserializedPacket;
        }

        /// <summary>
        ///     Converts for instance -1.12.1.8.-1.-1.-1.-1.-1 to eg. List<byte?> </summary>
        /// <param name="currentValues">String to convert</param>
        /// <param name="genericListType">
        ///     Type
        ///     of the property to convert
        /// </param>
        /// <returns>The string as converted List</returns>
        private static IList DeserializeSimpleList(string currentValues, Type genericListType)
        {
            var subpackets = (IList)Convert.ChangeType(Activator.CreateInstance(genericListType), genericListType);
            IEnumerable<string> splittedValues = currentValues.Split('.');

            foreach (string currentValue in splittedValues)
            {
                object value = DeserializeValue(genericListType.GenericTypeArguments[0], currentValue, null, null);
                subpackets.Add(value);
            }

            return subpackets;
        }

        private static object DeserializeSubpacket(string currentSubValues, Type packetBasePropertyType,
            KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo, bool isReturnPacket = false)
        {
            string[] subpacketValues = currentSubValues.Split(isReturnPacket ? '^' : '.');
            object newSubpacket = Activator.CreateInstance(packetBasePropertyType);

            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> subpacketPropertyInfo in subpacketSerializationInfo.Value)
            {
                int currentSubIndex = isReturnPacket ? subpacketPropertyInfo.Key.Index + 1 : subpacketPropertyInfo.Key.Index; // return packets do include header
                string currentSubValue = subpacketValues[currentSubIndex];

                subpacketPropertyInfo.Value.SetValue(newSubpacket, DeserializeValue(subpacketPropertyInfo.Value.PropertyType, currentSubValue, subpacketPropertyInfo.Key, null));
            }

            return newSubpacket;
        }

        /// <summary>
        ///     Converts a Sublist of Packets, For instance 0.4903.5.0.0 2.340.0.0.0
        ///     3.720.0.0.0 5.4912.6.0.0 9.227.0.0.0 10.803.0.0.0 to
        /// </summary>
        /// <param name="currentValue">The value as String</param>
        /// <param name="packetBasePropertyType">Type of the Property to convert to</param>
        /// <param name="shouldRemoveSeparator"></param>
        /// <param name="packetMatchCollections"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private static IList DeserializeSubpackets(string currentValue, Type packetBasePropertyType, bool shouldRemoveSeparator, MatchCollection packetMatchCollections, int? currentIndex)
        {
            // split into single values
            List<string> splittedSubpackets = currentValue.Split(' ').ToList();

            // generate new list
            var subpackets = (IList)Convert.ChangeType(Activator.CreateInstance(packetBasePropertyType), packetBasePropertyType);

            Type subPacketType = packetBasePropertyType.GetGenericArguments()[0];
            KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo = GetSerializationInformation(subPacketType);

            // handle subpackets with separator
            if (shouldRemoveSeparator)
            {
                if (!currentIndex.HasValue || packetMatchCollections == null)
                {
                    return subpackets;
                }

                List<string> splittedSubpacketParts = packetMatchCollections.Cast<Match>().Select(m => m.Value).ToList();
                splittedSubpackets = new List<string>();

                string generatedPseudoDelimitedString = string.Empty;
                int subPacketTypePropertiesCount = subpacketSerializationInfo.Value.Count;

                // check if the amount of properties can be serialized properly
                if ((splittedSubpacketParts.Count
                    % subPacketTypePropertiesCount) == 0) // amount of properties per subpacket does match the given value amount in %
                {
                    for (int i = currentIndex.Value + 1; i < splittedSubpacketParts.Count; i++)
                    {
                        int j;
                        for (j = i; j < i + subPacketTypePropertiesCount; j++)
                        {
                            // add delimited value
                            generatedPseudoDelimitedString += splittedSubpacketParts[j] + ".";
                        }

                        i = j - 1;

                        //remove last added separator
                        generatedPseudoDelimitedString = generatedPseudoDelimitedString.Substring(0, generatedPseudoDelimitedString.Length - 1);

                        // add delimited values to list of values to serialize
                        splittedSubpackets.Add(generatedPseudoDelimitedString);
                        generatedPseudoDelimitedString = string.Empty;
                    }
                }
                else
                {
                    throw new Exception("The amount of splitted subpacket values without delimiter do not match the % property amount of the serialized type.");
                }
            }

            foreach (string subpacket in splittedSubpackets)
            {
                subpackets.Add(DeserializeSubpacket(subpacket, subPacketType, subpacketSerializationInfo));
            }

            return subpackets;
        }

        private static object DeserializeValue(Type packetPropertyType, string currentValue, PacketIndexAttribute packetIndexAttribute, MatchCollection packetMatches)
        {
            // enum should be casted to number
            if (packetPropertyType.BaseType != null && packetPropertyType.BaseType == typeof(Enum))
            {
                object convertedValue = null;
                try
                {
                    if (currentValue != null && packetPropertyType.IsEnumDefined(Enum.Parse(packetPropertyType, currentValue)))
                    {
                        convertedValue = Enum.Parse(packetPropertyType, currentValue);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Could not convert value {currentValue} to type {packetPropertyType.Name}");
                }

                return convertedValue;
            }

            if (packetPropertyType == typeof(bool)) // handle boolean values
            {
                return currentValue != "0";
            }

            if (packetPropertyType.BaseType != null && packetPropertyType.BaseType == typeof(PacketDefinition)) // subpacket
            {
                KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo = GetSerializationInformation(packetPropertyType);
                return DeserializeSubpacket(currentValue, packetPropertyType, subpacketSerializationInfo, packetIndexAttribute?.IsReturnPacket ?? false);
            }

            if (packetPropertyType.IsGenericType && packetPropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) // subpacket list
                && packetPropertyType.GenericTypeArguments[0].BaseType == typeof(PacketDefinition))
            {
                return DeserializeSubpackets(currentValue, packetPropertyType, packetIndexAttribute?.RemoveSeparator ?? false, packetMatches, packetIndexAttribute?.Index);
            }

            if (packetPropertyType.IsGenericType && packetPropertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) // simple list
            {
                return DeserializeSimpleList(currentValue, packetPropertyType);
            }

            if (Nullable.GetUnderlyingType(packetPropertyType) != null && string.IsNullOrEmpty(currentValue)) // empty nullable value
            {
                return null;
            }

            if (Nullable.GetUnderlyingType(packetPropertyType) != null) // nullable value
            {
                if (packetPropertyType.GenericTypeArguments[0]?.BaseType == typeof(Enum))
                {
                    return Enum.Parse(packetPropertyType.GenericTypeArguments[0], currentValue);
                }

                return Convert.ChangeType(currentValue, packetPropertyType.GenericTypeArguments[0]);
            }

            return Convert.ChangeType(currentValue, packetPropertyType); // cast to specified type
        }

        private static void GenerateSerializationInformations<TPacketDefinition>()
        where TPacketDefinition : PacketDefinition
        {
            _packetSerializationInformations = new Dictionary<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>>();

            // Iterate thru all PacketDefinition implementations
            foreach (Type packetBaseType in typeof(TPacketDefinition).Assembly.GetTypes().Where(p => !p.IsInterface && typeof(TPacketDefinition).BaseType.IsAssignableFrom(p)))
            {
                // add to serialization informations
                KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> serializationInformations =
                    GenerateSerializationInformations(packetBaseType);
            }
        }

        private static KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> GenerateSerializationInformations(Type serializationType)
        {
            string header = serializationType.GetCustomAttribute<PacketHeaderAttribute>()?.Identification;

            if (string.IsNullOrEmpty(header))
            {
                throw new Exception($"Packet header cannot be empty. PacketType: {serializationType.Name}");
            }

            Dictionary<PacketIndexAttribute, PropertyInfo> packetsForPacketDefinition = new Dictionary<PacketIndexAttribute, PropertyInfo>();

            foreach (PropertyInfo packetBasePropertyInfo in serializationType.GetProperties().Where(x => x.GetCustomAttributes(false).OfType<PacketIndexAttribute>().Any()))
            {
                PacketIndexAttribute indexAttribute = packetBasePropertyInfo.GetCustomAttributes(false).OfType<PacketIndexAttribute>().FirstOrDefault();

                if (indexAttribute != null)
                {
                    packetsForPacketDefinition.Add(indexAttribute, packetBasePropertyInfo);
                }
            }

            // order by index
            IOrderedEnumerable<KeyValuePair<PacketIndexAttribute, PropertyInfo>> keyValuePairs = packetsForPacketDefinition.OrderBy(p => p.Key.Index);

            KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> serializationInformatin =
                new KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>>(new Tuple<Type, string>(serializationType, header), packetsForPacketDefinition);
            if (_packetSerializationInformations == null)
            {
                _packetSerializationInformations = new Dictionary<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>>();
            }
            _packetSerializationInformations.Add(serializationInformatin.Key, serializationInformatin.Value);

            return serializationInformatin;
        }

        private static KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> GetSerializationInformation(Type serializationType)
        {
            if (_packetSerializationInformations != null && _packetSerializationInformations.Any(si => si.Key.Item1 == serializationType))
            {
                return _packetSerializationInformations.SingleOrDefault(si => si.Key.Item1 == serializationType);
            }
            return GenerateSerializationInformations(serializationType); // generic runtime serialization parameter generation
        }

        /// <summary>
        ///     Converts for instance List<byte?> to -1.12.1.8.-1.-1.-1.-1.-1
        /// </summary>
        /// <param
        ///     name="listValues">
        ///     Values in List of simple type.
        /// </param>
        /// <param name="propertyType">
        ///     The
        ///     simple type.
        /// </param>
        /// <returns></returns>
        private static string SerializeSimpleList(IList listValues, Type propertyType)
        {
            string resultListPacket = string.Empty;
            int listValueCount = listValues.Count;
            if (listValueCount > 0)
            {
                resultListPacket += SerializeValue(propertyType.GenericTypeArguments[0], listValues[0]);

                for (int i = 1; i < listValueCount; i++)
                {
                    resultListPacket += $".{SerializeValue(propertyType.GenericTypeArguments[0], listValues[i]).Replace(" ", "")}";
                }
            }

            return resultListPacket;
        }

        private static string SerializeSubpacket(object value, KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo, bool isReturnPacket,
            bool shouldRemoveSeparator)
        {
            string serializedSubpacket = isReturnPacket ? $" #{subpacketSerializationInfo.Key.Item2}^" : " ";

            // iterate thru configure subpacket properties
            foreach (KeyValuePair<PacketIndexAttribute, PropertyInfo> subpacketPropertyInfo in subpacketSerializationInfo.Value)
            {
                // first element
                if (subpacketPropertyInfo.Key.Index != 0)
                {
                    serializedSubpacket += isReturnPacket ? "^" :
                        shouldRemoveSeparator ? " " : ".";
                }

                serializedSubpacket += SerializeValue(subpacketPropertyInfo.Value.PropertyType, subpacketPropertyInfo.Value.GetValue(value)).Replace(" ", "");
            }

            return serializedSubpacket;
        }

        private static string SerializeSubpackets(ICollection listValues, Type packetBasePropertyType, bool shouldRemoveSeparator)
        {
            string serializedSubPacket = string.Empty;
            KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo = GetSerializationInformation(packetBasePropertyType.GetGenericArguments()[0]);

            if (listValues.Count > 0)
            {
                serializedSubPacket = listValues.Cast<object>().Aggregate(serializedSubPacket,
                    (current, listValue) => current + SerializeSubpacket(listValue, subpacketSerializationInfo, false, shouldRemoveSeparator));
            }

            return serializedSubPacket;
        }

        private static string SerializeValue(Type propertyType, object value, PacketIndexAttribute packetIndexAttribute = null)
        {
            if (propertyType == null)
            {
                return string.Empty;
            }

            // check for nullable without value or string
            if (propertyType == typeof(string) && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return " -";
            }

            if (Nullable.GetUnderlyingType(propertyType) != null && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return " -1";
            }

            // enum should be casted to number
            if (propertyType.BaseType != null && propertyType.BaseType == typeof(Enum))
            {
                return $" {Convert.ToInt16(value)}";
            }

            if (propertyType == typeof(bool))
            {
                // bool is 0 or 1 not True or False
                return Convert.ToBoolean(value) ? " 1" : " 0";
            }

            if (propertyType.BaseType != null && propertyType.BaseType == typeof(PacketDefinition))
            {
                KeyValuePair<Tuple<Type, string>, Dictionary<PacketIndexAttribute, PropertyInfo>> subpacketSerializationInfo = GetSerializationInformation(propertyType);
                return SerializeSubpacket(value, subpacketSerializationInfo, packetIndexAttribute?.IsReturnPacket ?? false, packetIndexAttribute?.RemoveSeparator ?? false);
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))
                && propertyType.GenericTypeArguments[0].BaseType == typeof(PacketDefinition))
            {
                return SerializeSubpackets((IList)value, propertyType, packetIndexAttribute?.RemoveSeparator ?? false);
            }

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>))) //simple list
            {
                return SerializeSimpleList((IList)value, propertyType);
            }

            return $" {value}";
        }

        private static void SetDeserializationInformations(PacketDefinition packetDefinition, string packetContent, string packetHeader)
        {
            packetDefinition.OriginalContent = packetContent;
            packetDefinition.OriginalHeader = packetHeader;
        }

        #endregion
    }
}
