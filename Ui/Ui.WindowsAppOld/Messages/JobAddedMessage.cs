namespace codingfreaks.pping.Ui.WindowsApp.Messages
{
    using System;
    using System.Linq;

    using Models;

    using ViewModel;

    /// <summary>
    /// Is sent by the <see cref="AddJobViewModel"/> to inform the app to add a new job.
    /// </summary>
    public class JobAddedMessage
    {
        #region constructors and destructors

        public JobAddedMessage(JobModel newJob)
        {
            NewJob = newJob;
        }

        #endregion

        #region properties

        /// <summary>
        /// The data of the job definition.
        /// </summary>
        public JobModel NewJob { get; }

        #endregion
    }
}