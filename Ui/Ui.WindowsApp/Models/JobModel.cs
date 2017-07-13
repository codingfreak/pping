namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using cfUtils.Logic.Portable.Extensions;

    public class JobModel : BaseModel
    {
        #region constructors and destructors

        public JobModel(bool useTcp = true)
        {
            Tcp = useTcp;
            Udp = !useTcp;
        }

        #endregion

        #region properties

        public bool AutoStop { get; set; }

        public DateTimeOffset? Finished { get; set; }

        public bool IsValid => !TargetAddess.IsNullOrEmpty() && TargetPorts.Any() && Tcp != Udp;

        public TimeSpan? MaxRuntime { get; set; }

        public int? MaxTries { get; set; }

        public string NetworkType => Tcp ? "TCP" : "UDP";

        public DateTimeOffset PlannedStart { get; set; }

        public DateTimeOffset? Started { get; set; }

        public string TargetAddess { get; set; }

        public IEnumerable<int> TargetPorts { get; set; } = Enumerable.Empty<int>();

        public string TargetPortsFormatted
        {
            get => string.Join(",", TargetPorts ?? Enumerable.Empty<int>());
            set
            {
                if (value.IsNullOrEmpty())
                {
                    TargetPorts = Enumerable.Empty<int>();
                }
                try
                {
                    var result = new List<int>();
                    var parts = value.Split(',');
                    if (parts.Any())
                    {
                        foreach (var part in parts)
                        {
                            if (int.TryParse(part, NumberStyles.Integer, CultureInfo.CurrentUICulture, out var parsed))
                            {
                                result.Add(parsed);
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    TargetPorts = result;
                }
                catch
                {
                    // empty catch                    
                }
            }
        }

        public bool Tcp { get; }

        public bool Udp { get; }

        #endregion
    }
}