namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// Contains the data of a single pping job that is currently running or comes from history.
    /// </summary>
    public class JobResultModel : BaseModel
    {
        #region properties

        /// <summary>
        /// The amount of closed ports.
        /// </summary>
        [JsonIgnore]
        public int Closed => Runs.Count(r => !r.PortReached);

        /// <summary>
        /// The amount of opened ports.
        /// </summary>
        [JsonIgnore]
        public int Opened => Runs.Count(r => r.PortReached);
        
        /// <summary>
        /// The list of operations performed and logged.
        /// </summary>
        public ObservableCollection<JobSingleRunModel> Runs { get; set; } = new ObservableCollection<JobSingleRunModel>();

        #endregion
    }
}