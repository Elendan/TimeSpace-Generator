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
    }
}
