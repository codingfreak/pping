namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Linq;

    public class JobModel
    {
        #region properties

        public JobModel(bool useTcp = true)
        {
            Tcp = useTcp;
            Udp = !useTcp;
        }

        public bool AutoStop { get; set; }

        public DateTimeOffset? Finished { get; set; }

        public TimeSpan? MaxRuntime { get; set; }

        public int? MaxTries { get; set; }

        public string NetworkType => Tcp ? "TCP" : "UDP";

        public DateTimeOffset PlannedStart { get; set; }

        public DateTimeOffset? Started { get; set; }

        public string TargetAddess { get; set; }

        public int[] TargetPorts { get; set; }

        public string TargetPortsFormatted => string.Join(",", TargetPorts);

        public bool Tcp { get; }

        public bool Udp { get;  }

        #endregion
    }
}