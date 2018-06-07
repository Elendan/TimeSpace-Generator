using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Helpers
{
    public class ScriptHelper
    {
        #region Singleton
        private static ScriptHelper _instance;

        public static ScriptHelper Instance => _instance ?? (_instance = new ScriptHelper());
        #endregion

        public List<short> Buttons = new List<short>()
        {
             1000,
             1051,
             1055,
             1057
        };

        public string[] Space { get; set; }

        public ScriptHelper()
        {
            this.Space = new string[20];
            this.Space[0] = "   ";
            for (int index = 1; index < this.Space.Length; ++index)
                this.Space[index] = this.Space[index - 1] + this.Space[0];
        }

    }
}
