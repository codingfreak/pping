namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using cfUtils.Logic.Portable.Extensions;

    using Enumerations;

    using EventArguments;

    using Logic;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines the data for a single pping-Job.
    /// </summary>
    public class JobModel : BaseModel
    {
        #region member vars

        private PingJob _currentJob;

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

        #region methods

        /// <summary>
        /// Starts a new job
        /// </summary>
        public PingJob StartNew()
        {
            var job = new PingJob(this);
            Jobs.Add(job);
            CurrentJob = job;
            job.Start();
            return job;
        }

        private void OnCurrentJobResultReceived(object sender, JobResultEventArgs e)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(CurrentJob));
        }

        private void OnCurrentStateChanged(object sender, EventArgs e)
        {
            if (CurrentJob.State == JobStateEnum.Finished)
            {
                CurrentJob = null;
            }
            // ReSharper disable once ExplicitCallerInfoArgument 
            OnPropertyChanged(nameof(CurrentJob));
            // ReSharper disable once ExplicitCallerInfoArgument 
            OnPropertyChanged(nameof(StartStopCaption));
            // ReSharper disable once ExplicitCallerInfoArgument 
            OnPropertyChanged(nameof(IsBusy));
            // ReSharper disable once ExplicitCallerInfoArgument 
            OnPropertyChanged(nameof(StateText));
            // ReSharper disable once ExplicitCallerInfoArgument 
            OnPropertyChanged(nameof(State));
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
        /// The currently active job if any.
        /// </summary>
        [JsonIgnore]
        public PingJob CurrentJob
        {
            get => _currentJob;
            private set
            {
                if (_currentJob != null)
                {
                    _currentJob.ResultReceived -= OnCurrentJobResultReceived;
                    _currentJob.StateChanged -= OnCurrentStateChanged;
                }
                _currentJob = value;
                if (_currentJob != null)
                {
                    _currentJob.ResultReceived += OnCurrentJobResultReceived;
                    _currentJob.StateChanged += OnCurrentStateChanged;
                }
            }
        }

        /// <summary>
        /// Indicates if a jib is running currently.
        /// </summary>
        public bool IsBusy => CurrentJob != null;

        /// <summary>
        /// Indicates whether the current definition of the job is consistent.
        /// </summary>
        [Browsable(false)]
        public bool IsValid => !TargetAddess.IsNullOrEmpty() && TargetPorts.Any() && Tcp != Udp;

        /// <summary>
        /// The list of jobs associated with this definition.
        /// </summary>
        public ObservableCollection<PingJob> Jobs { get; set; } = new ObservableCollection<PingJob>();

        /// <summary>
        /// Defines an optional maximum rutime for jobs of this definition.
        /// </summary>
        [Category("Behavior")]
        [DisplayName("Maximum Runtime")]
        [Description("Defines an optional maximum rutime for jobs of this definition.")]
        public TimeSpan? MaxRuntime { get; set; }

        /// <summary>
        /// Defines an optional maximum amount of pping operations of this definition.
        /// </summary>
        /// <remarks>
        /// Defaults to 4.
        /// </remarks>
        [Category("Behavior")]
        [DisplayName("Maximum Pings")]
        [Description("Defines an optional maximum amount of pping operations of this definition.")]
        public int? MaxTries { get; set; } = 4;

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
        /// Defines if IP address resolving should be perfomed on every run.
        /// </summary>
        [Category("Misc")]
        [DisplayName("Resolve IP address")]
        [Description("Defines if IP address resolving should be perfomed on every run.")]
        public bool ResolveAddress { get; set; }

        /// <summary>
        /// The caption for start-stop-commands.
        /// </summary>
        [Browsable(false)]
        public string StartStopCaption => CurrentJob == null ? "Start" : "Stop";

        /// <summary>
        /// The state of the current jobs if any.
        /// </summary>
        [Browsable(false)]
        public JobStateEnum State => CurrentJob?.State ?? JobStateEnum.Unkown;

        /// <summary>
        /// The state text of the current jobs if any.
        /// </summary>
        [Browsable(false)]
        public string StateText => CurrentJob?.State.ToString() ?? "-";

        /// <summary>
        /// The target host name or IP address to use.
        /// </summary>
        [Category("Basic")]
        [DisplayName("Target Address")]
        [Description("The target host name or IP address to use.")]
        public string TargetAddess { get; set; }

        /// <summary>
        /// The list of ports to check on the <see cref="TargetAddess" />.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<int> TargetPorts { get; set; } = Enumerable.Empty<int>();

        /// <summary>
        /// The readable version of <see cref="TargetPorts" />.
        /// </summary>
        [Browsable(false)]
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
        /// Defines the maximum timeout for a single ping.
        /// </summary>
        /// <remarks>
        /// Defaults to 1 second.
        /// </remarks>
        [Category("Behavior")]
        [DisplayName("Timeout")]
        [Description("The timout to apply for each network operation. The port is considred as unreachable after this time period.")]
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(1);

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
        [Category("Behavior")]
        [DisplayName("Wait time")]
        [Description("Defines the pause between each test run.")]
        public TimeSpan WaitTime { get; set; } = TimeSpan.FromSeconds(10);

        #endregion
    }
}