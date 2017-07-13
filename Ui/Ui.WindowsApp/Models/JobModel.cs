namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using cfUtils.Logic.Portable.Extensions;

    using GalaSoft.MvvmLight.Command;

    /// <summary>
    /// Defines the data for a single pping-Job.
    /// </summary>
    public class JobModel : BaseModel
    {
        #region member vars

        private bool _useTcp;

        private bool _useUdp;

        #endregion

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
        [Category("Behavior")]
        [DisplayName("Stop on first open")]
        [Description("Indicates whether the job should stop when the first successful port ping arrived.")]
        public bool AutoStop { get; set; }

        /// <summary>
        /// Indicates whether the current definition of the job is consistent.
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Category("Basic")]
        [DisplayName("Use TCP")]
        [Description("Indicates whether TCP should be used.")]
        public bool Tcp
        {
            get => _useTcp;
            set
            {
                _useTcp = value;
                _useUdp = !value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Udp));
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(NetworkType));
            }
        }

        /// <summary>
        /// Indicates whether UDP should be used.
        /// </summary>
        [Category("Basic")]
        [DisplayName("Use UDP")]
        [Description("Indicates whether UDP should be used.")]
        public bool Udp
        {
            get => _useUdp;
            set
            {
                _useUdp = value;
                _useTcp = !value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(Tcp));
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(NetworkType));
            }
        }

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