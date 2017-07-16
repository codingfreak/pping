namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Linq;

    public class JobSingleRunModel : BaseModel
    {
        #region properties

        public int Port { get; set; }

        public bool PortReached { get; set; }

        public string ResolvedAddress { get; set; }

        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

        #endregion
    }
}