namespace codingfreaks.pping.Ui.WindowsApp.EventArguments
{
    using System;
    using System.Linq;

    using Models;

    public class JobResultEventArgs : EventArgs
    {
        #region constructors and destructors

        public JobResultEventArgs(JobSingleRunModel result)
        {
            Result = result;
        }

        #endregion

        #region properties

        public JobSingleRunModel Result { get; }

        #endregion
    }
}