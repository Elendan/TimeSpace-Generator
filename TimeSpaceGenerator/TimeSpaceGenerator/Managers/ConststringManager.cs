using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeSpaceGenerator.Managers
{
    public static class ConststringManager
    {
        public static Dictionary<int, string> StringsDict = new Dictionary<int, string>();

        public static void ImportStrings()
        {
            if (!File.Exists("conststring.dat"))
            {
                MessageBox.Show("You need put the conststring.dat in the bin folder.");
                Environment.Exit(1);
            }
            string line;
            using (StreamReader Reader = File.OpenText("conststring.dat"))
            {
                while ((line = Reader.ReadLine()) != null)
                {
                    string[] content = line.Split((char)0x0b);
                    int Index = Convert.ToInt32(content[0]);
                    if (Index > 10000)
                    {
                        StringsDict.Add(Index - 10000, content[1]);
                    }
                }
            }
        }

        public static string GetString(int index)
        {
            return StringsDict[index];
        }
    }
}
