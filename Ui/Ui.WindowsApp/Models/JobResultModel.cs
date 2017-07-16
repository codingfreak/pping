namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    public class JobResultModel : BaseModel
    {
        #region properties

        [JsonIgnore]
        public int Closed => Runs.Count(r => !r.PortReached);

        [JsonIgnore]
        public int Opened => Runs.Count(r => r.PortReached);

        public List<JobSingleRunModel> Runs { get; } = new List<JobSingleRunModel>();

        #endregion
    }
}