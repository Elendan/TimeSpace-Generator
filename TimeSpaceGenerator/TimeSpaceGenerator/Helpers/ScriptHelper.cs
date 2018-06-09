using System.Collections.Generic;

namespace TimeSpaceGenerator.Helpers
{
    public class ScriptHelper
    {
        public List<short> Buttons = new List<short>
        {
            1000,
            1051,
            1055,
            1057
        };

        #region Singleton

        private static ScriptHelper _instance;

        public static ScriptHelper Instance => _instance ?? (_instance = new ScriptHelper());

        #endregion
    }
}