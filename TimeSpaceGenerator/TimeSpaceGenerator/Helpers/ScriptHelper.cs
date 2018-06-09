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
    }
}
