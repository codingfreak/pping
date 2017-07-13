namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using cfUtils.Logic.Portable.Extensions;

    /// <summary>
    /// Defines the data for a single pping-Job.
    /// </summary>
    public class JobModel : BaseModel
    {
        #region constructors and destructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="useTcp"><c>true</c> if TCP should be used otherwise <c>false</c> for UDP.</param>
        public JobModel(bool useTcp = true)
        {
            Tcp = useTcp;
            Udp = !useTcp;
        }

        #endregion

        #region properties

        /// <summary>
        /// Indicates whether the job should stop when the first successful port ping arrived.
        /// </summary>
        public bool AutoStop { get; set; }

        /// <summary>
        /// Indicates whether the current definition of the job is consistent.
        /// </summary>
        public bool IsValid => !TargetAddess.IsNullOrEmpty() && TargetPorts.Any() && Tcp != Udp;

        /// <summary>
        /// Defines an optional maximum rutime for jobs of this definition.
        /// </summary>
        public TimeSpan? MaxRuntime { get; set; }

        /// <summary>
        /// Defines an optional maximum amount of pping operations of this definition.
        /// </summary>
        public int? MaxTries { get; set; }

        /// <summary>
        /// Shows the string representation of the network type used by jobs (TCP or UDP).
        /// </summary>
        public string NetworkType => Tcp ? "TCP" : "UDP";

        /// <summary>
        /// Defines an optional fixed time to start the pping.
        /// </summary>
        public DateTimeOffset? PlannedStart { get; set; }

        /// <summary>
        /// The target host name or IP address to use.
        /// </summary>
        public string TargetAddess { get; set; }

        /// <summary>
        /// The list of ports to check on the <see cref="TargetAddess" />.
        /// </summary>
        public IEnumerable<int> TargetPorts { get; set; } = Enumerable.Empty<int>();

        /// <summary>
        /// The readable version of <see cref="TargetPorts" />.
        /// </summary>
        public string TargetPortsFormatted => string.Join(",", TargetPorts ?? Enumerable.Empty<int>());

        /// <summary>
        /// Indicates whether TCP should be used.
        /// </summary>
        public bool Tcp { get; }

        /// <summary>
        /// Indicates whether UDP should be used.
        /// </summary>
        public bool Udp { get; }

        /// <summary>
        /// Defines the pause between 2 ppings.
        /// </summary>
        /// <remarks>
        /// Defaults to 10 seconds.
        /// </remarks>
        public TimeSpan WaitTime { get; set; } = TimeSpan.FromSeconds(10);

        #endregion
    }
}