namespace codingfreaks.pping.Ui.WindowsApp.Logic
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using cfUtils.Logic.Base.Utilities;

    using Enumerations;

    using EventArguments;

    using GalaSoft.MvvmLight.Threading;

    using Models;

    using Newtonsoft.Json;

    public class PingJob : BaseModel
    {
        #region member vars

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

        private JobStateEnum _state;

        #endregion

        #region events

        /// <summary>
        /// Occurs when the <see cref="State" /> changed its value.
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// Occurs when a single result is received.
        /// </summary>
        public event EventHandler<JobResultEventArgs> ResultReceived;

        #endregion

        #region constructors and destructors

        public PingJob(JobModel jobDefinition)
        {
            JobDefinition = jobDefinition;
            State = JobStateEnum.Defined;
            Result = new JobResultModel();
        }

        #endregion

        #region methods

        /// <summary>
        /// Starts the job from outside.
        /// </summary>
        public void Start()
        {
            if (Started.HasValue)
            {
                throw new InvalidOperationException("This job has already been started.");
            }
            Started = DateTimeOffset.Now;
            State = JobStateEnum.Starting;
            Task.Run(Run, _tokenSource.Token).ContinueWith(
                t =>
                {
                    if (!Finished.HasValue)
                    {
                        Stop();
                    }
                });
        }

        /// <summary>
        /// Stops a currently running job.
        /// </summary>
        public void Stop()
        {
            if (!Started.HasValue)
            {
                throw new InvalidOperationException("Cannot stop a job which hasn't been started.");
            }
            if (Finished.HasValue)
            {
                throw new InvalidOperationException("This job has already been stopped.");
            }
            State = JobStateEnum.Stopping;
            _tokenSource.Cancel();
            Finished = DateTimeOffset.Now;
            State = JobStateEnum.Finished;
        }

        /// <summary>
        /// Internal representation of the job logic.
        /// </summary>
        private async Task Run()
        {
            var runs = 0;
            var shouldRun = true;
            var overallWatch = new Stopwatch();
            var portWatch = new Stopwatch();
            overallWatch.Start();
            while (!_tokenSource.IsCancellationRequested && shouldRun)
            {
                runs++;
                State = JobStateEnum.Running;
                JobDefinition.TargetPorts.ToList().ForEach(
                    port =>
                    {
                        try
                        {
                            var runResult = new JobSingleRunModel
                            {
                                Tcp = JobDefinition.Tcp,
                                Udp = JobDefinition.Udp,
                                Port = port
                            };
                            portWatch.Restart();
                            runResult.PortReached = NetworkUtil.IsPortOpened(JobDefinition.TargetAddess, port, (int)JobDefinition.Timeout.TotalSeconds, JobDefinition.Udp);
                            portWatch.Stop();
                            runResult.Duration = portWatch.Elapsed;
                            if (JobDefinition.ResolveAddress)
                            {
                                // job should try to resolve ip address
                                Dns.BeginGetHostEntry(
                                    JobDefinition.TargetAddess,
                                    ar =>
                                    {
                                        IPHostEntry firstNetworkAddress = null;
                                        try
                                        {
                                            firstNetworkAddress = Dns.EndGetHostEntry(ar);
                                        }
                                        catch
                                        {
                                            // empty catch
                                        }
                                        if (firstNetworkAddress == null || !firstNetworkAddress.AddressList.Any())
                                        {
                                            return;
                                        }
                                        runResult.ResolvedAddress = firstNetworkAddress.AddressList[0].ToString();
                                    },
                                    null);
                            }
                            DispatcherHelper.UIDispatcher.Invoke(() => Result.Runs.Add(runResult));
                            ResultReceived?.Invoke(this, new JobResultEventArgs(runResult));
                            if (runResult.PortReached && JobDefinition.AutoStop)
                            {
                                // the port is reached and autostop is on
                                shouldRun = false;
                                return;
                            }
                            if (JobDefinition.MaxTries.HasValue && JobDefinition.MaxTries.Value <= runs)
                            {
                                // the maximum amount of tries is reached
                                shouldRun = false;
                                return;                                
                            }
                            if (JobDefinition.MaxRuntime.HasValue && JobDefinition.MaxRuntime.Value <= overallWatch.Elapsed)
                            {
                                // maximum runtime is reached
                                shouldRun = false;
                                return;
                            }
                            // inform callers that there is a new result
                            // ReSharper disable once ExplicitCallerInfoArgument
                            OnPropertyChanged(nameof(Result));                            
                        }
                        catch (Exception ex)
                        {
                            // TODO
                        }
                    });
                State = JobStateEnum.Waiting;
                await Task.Delay(JobDefinition.WaitTime, CancellationToken.None);
            }
            overallWatch.Stop();            
        }

        #endregion

        #region properties

        /// <summary>
        /// The timestamp on which the job was finished.
        /// </summary>
        public DateTimeOffset? Finished { get; set; }

        /// <summary>
        /// The formatted version of <see cref="Finished" />.
        /// </summary>
        public string FinishedFormatted => Finished?.ToLocalTime().ToString("G");

        /// <summary>
        /// The definition for the current job containing all settings.
        /// </summary>
        [JsonIgnore]
        public JobModel JobDefinition { get; }

        /// <summary>
        /// The result containing all collected data.
        /// </summary>
        public JobResultModel Result { get; set; }

        /// <summary>
        /// The timestamp on which the job was started.
        /// </summary>
        public DateTimeOffset? Started { get; set; }

        /// <summary>
        /// The formatted version of <see cref="Started" />.
        /// </summary>
        public string StartedFormatted => Started?.ToLocalTime().ToString("G");

        /// <summary>
        /// The current state of the jobs.
        /// </summary>
        public JobStateEnum State
        {
            get => _state;
            set
            {
                if (value == _state)
                {
                    return;
                }
                _state = value;
                OnPropertyChanged();
                StateChanged?.Invoke(this, EventArgs.Empty);                
            }
        }

        #endregion
    }
}