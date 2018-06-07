using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Helpers
{
    public class PacketHelper
    {
        #region Singleton

        private static PacketHelper _instance;

        public static PacketHelper Instance => _instance ?? (_instance = new PacketHelper());

        #endregion

        #region Instantiation

        public PacketHelper()
        {
            ReplacableChars = new List<char>();
            ReplacableChars.AddRange(new List<char> {'.'});
        }

        #endregion

        #region Properties

        public List<char> ReplacableChars { get; set; }

        #endregion

        #region Methods

        public int RemovableStringIndex(string packet, char delim1, char delim2, byte count1, byte count2)
        {
            int j = 0, k = 0;
            if (char.IsLetter(packet[0]))
            {
                return 0;
            }

            for (int i = 0; i < packet.Length; i++)
            {
                if (packet[i] == delim1)
                {
                    j++;
                }

                if (packet[i] == delim2)
                {
                    k++;
                }

                if (j == count1 && k == count2 && char.IsLetter(packet[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public string FormatPacket(string packet, char token)
        {
            foreach (char c in ReplacableChars)
            {
                packet = packet.Replace(c, token);
            }

            return packet;
        }

        #endregion
    }
}
