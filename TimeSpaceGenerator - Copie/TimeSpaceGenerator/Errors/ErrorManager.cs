using System.Collections.Generic;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Errors
{
    public class ErrorManager
    {
        #region instantiation

        public ErrorManager()
        {
            Error = new List<KeyValuePair<ErrorType, string>>();
        }

        #endregion

        #region Members

        private static ErrorManager _instance;

        #endregion

        #region Properties

        public ErrorType Type { get; set; }

        public List<KeyValuePair<ErrorType, string>> Error { get; set; }

        #endregion

        #region Singleton

        public static ErrorManager Instance => _instance ?? (_instance = new ErrorManager());

        #endregion
    }
}
