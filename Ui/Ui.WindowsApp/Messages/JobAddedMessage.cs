namespace codingfreaks.pping.Ui.WindowsApp.Messages
{
    using System;
    using System.Linq;

    using Models;

    public class JobAddedMessage
    {
        #region constructors and destructors

        public JobAddedMessage(JobModel newJob)
        {
            NewJob = newJob;
        }

        #endregion

        #region properties

        public JobModel NewJob { get; }

        #endregion
    }
}