using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TimeSpaceGenerator.Enums;

namespace TimeSpaceGenerator.Errors
{
    public class ErrorManager
    {
        #region instantiation

        public ErrorManager() => Error = new List<KeyValuePair<ErrorType, string>>();

        #endregion

        #region Members

        private static ErrorManager _instance;

        #endregion

        #region Properties

        public ErrorType Type { get; set; }

        public List<KeyValuePair<ErrorType, string>> Error { get; set; }

        #endregion

        #region Methods

        public void AddError(ErrorType type, string error)
        {
            Error.Add(new KeyValuePair<ErrorType, string>(type, error));
        }

        public void Dump(TextBox errorTextBox)
        {
            foreach (KeyValuePair<ErrorType, string> error in Error)
            {
                errorTextBox.Text += $"{error.Key}: {error.Value}" + Environment.NewLine;
            }
        }

        #endregion

        #region Singleton

        public static ErrorManager Instance => _instance ?? (_instance = new ErrorManager());

        #endregion
    }
}
