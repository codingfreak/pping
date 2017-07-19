namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Newtonsoft.Json;

    public class JobResultModel : BaseModel
    {
        #region properties

        [JsonIgnore]
        public int Closed => Runs.Count(r => !r.PortReached);

        [JsonIgnore]
        public int Opened => Runs.Count(r => r.PortReached);
       
        public BindingList<JobSingleRunModel> Runs { get; set; } = new BindingList<JobSingleRunModel>();
       
        #endregion
    }
}