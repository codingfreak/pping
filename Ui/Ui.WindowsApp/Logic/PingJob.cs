namespace codingfreaks.pping.Ui.WindowsApp.Logic
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using cfUtils.Logic.Base.Utilities;

    using Enumerations;

    using EventArguments;

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
            while (!_tokenSource.IsCancellationRequested)
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
                                Port = port
                            };
                            runResult.PortReached = NetworkUtil.IsPortOpened(JobDefinition.TargetAddess, port, (int)JobDefinition.Timeout.TotalSeconds, JobDefinition.Udp);
                            if (JobDefinition.ResolveAddress)
                            {
                                Dns.BeginGetHostEntry(
                                    JobDefinition.TargetAddess,
                                    ar =>
                                    {
                                        IPHostEntry local0 = null;
                                        try
                                        {
                                            local0 = Dns.EndGetHostEntry(ar);
                                        }
                                        catch
                                        {
                                            // empty catch
                                        }
                                        if (local0 == null || !local0.AddressList.Any())
                                        {
                                            return;
                                        }
                                        runResult.ResolvedAddress = local0.AddressList[0].ToString();
                                    },
                                    null);
                            }
                            Result.Runs.Add(runResult);
                            ResultReceived?.Invoke(this, new JobResultEventArgs(runResult));
                            if (runResult.PortReached && JobDefinition.AutoStop)
                            {
                                return;
                            }
                            if (JobDefinition.MaxTries.HasValue && JobDefinition.MaxTries.Value <= runs)
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            // TODO
                        }
                    });
                State = JobStateEnum.Waiting;
                await Task.Delay(JobDefinition.WaitTime, CancellationToken.None);
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// The timestamp on which the job was finished.
        /// </summary>
        public DateTimeOffset? Finished
        {
            get;
            private set;
        }

        /// <summary>
        /// The definition for the current job containing all settings.
        /// </summary>
        [JsonIgnore]
        public JobModel JobDefinition { get; }

        /// <summary>
        /// The result containing all collected data.
        /// </summary>
        public JobResultModel Result { get; } = new JobResultModel();

        /// <summary>
        /// The timestamp on which the job was started.
        /// </summary>
        public DateTimeOffset? Started { get; private set; }

        /// <summary>
        /// The current state of the jobs.
        /// </summary>
        public JobStateEnum State
        {
            get => _state;
            private set
            {
                if (value == _state)
                {
                    return;
                }
                _state = value;                                
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}