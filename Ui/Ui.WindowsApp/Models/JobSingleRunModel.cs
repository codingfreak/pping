namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Linq;

    using Logic;

    public class JobSingleRunModel : BaseModel
    {
        #region properties

        public TimeSpan Duration { get; set; }

        public int Port { get; set; }

        public string PortPurpose
        {
            get
            {
                return Variables.KnownPorts.SingleOrDefault(p => p.Port == Port && p.Tcp == Tcp)?.Purpose ?? "-";
            }
        }

        public bool PortReached { get; set; }

        public string ResolvedAddress { get; set; }

        public bool Tcp { get; set; }

        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

        /// <summary>
        /// The formatted version of <see cref="Timestamp" />.
        /// </summary>
        public string TimestampFormatted => Timestamp.ToLocalTime().ToString("G");

        public bool Udp { get; set; }

        #endregion
    }
}