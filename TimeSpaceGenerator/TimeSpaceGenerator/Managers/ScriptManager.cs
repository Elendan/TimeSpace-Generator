using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSpaceGenerator.Managers
{
    public class ScriptManager
    {
        #region Members

        private static ScriptManager _instance;

        #endregion

        #region Instantiation

        #endregion

        #region Properties

        public static ScriptManager Instance => _instance ?? (_instance = new ScriptManager());

        #endregion

        #region Methods

        #endregion
    }
}
