namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    using cfUtils.Logic.Wpf.MvvmLight;

    using Enumerations;

    using GalaSoft.MvvmLight.Command;

    using Logic;

    using Messages;

    using Models;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines logic and data for the <see cref="MainWindow" />.
    /// </summary>
    /// <remarks>
    /// Don't rename this file to MainViewModel (without s) because ReSharper will not cleanup this file then.
    /// </remarks>
    public class MainViewsModel : BaseViewModel
    {
        #region member vars

        private readonly string _optionsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json");

        #endregion

        #region methods

        /// <summary>
        /// Provides possiblity to add jobs to the internal list.
        /// </summary>
        /// <param name="newJob"></param>
        public void AddJob(JobModel newJob)
        {
            JobDefinitions.Add(newJob);
        }

        /// <inheritdoc />
        protected override void InitCommands()
        {
            AddJobCommand = new RelayCommand(
                () =>
                {
                    MessengerInstance.Send(new AddJobWindowOpenMessage());
                });
            RemoveJobCommand = new RelayCommand<Window>(
                window =>
                {
                    if (window == null)
                    {
                        window = MessageListener.GetWindow(typeof(MainWindow));
                    }
                    if (MessageBox.Show(window, "Do you want to delete the job?", "Delete job", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        JobDefinitions.Remove(CurrentSelectedJobDefinition);                        
                    }
                },
                window => CurrentSelectedJobDefinition != null);
            ClosingCommand = new RelayCommand(
                () =>
                {
                    while (JobDefinitions.Any(j => j.CurrentJob != null && j.CurrentJob.State == JobStateEnum.Stopping))
                    {
                        Task.Delay(100).Wait();
                    }
                    JobDefinitions.Where(j => j.CurrentJob != null && !j.CurrentJob.Finished.HasValue).ToList().ForEach(
                        j =>
                        {
                            j.CurrentJob.Stop();
                        });
                    SaveCommand.Execute(null);
                });
            LoadedCommand = new RelayCommand(ReloadOptions);
            SaveCommand = new RelayCommand(
                () =>
                {
                    Options.JobDefinitions = JobDefinitions.ToList();
                    var fileContent = JsonConvert.SerializeObject(Options);
                    try
                    {
                        File.WriteAllText(_optionsFilePath, fileContent);
                    }
                    catch (Exception ex)
                    {
                    }
                });
            StartStopJobCommand = new RelayCommand<JobModel>(
                jobDef =>
                {
                    if (jobDef == null)
                    {
                        // don't know if this can happen
                        return;
                    }
                    if (jobDef.IsBusy)
                    {
                        jobDef.CurrentJob.Stop();
                        return;
                    }
                    CurrentSelectedPingJob = jobDef.StartNew();
                },
                job => CurrentSelectedJobDefinition != null);
            CleanJobHistoryCommand = new RelayCommand<JobModel>(
                jobDef =>
                {
                    if (jobDef == null)
                    {
                        // don't know if this can happen
                        return;
                    }
                    jobDef.Jobs.Clear();
                    JobDefinitionsView.Refresh();
                },
                job => CurrentSelectedJobDefinition != null && !CurrentSelectedJobDefinition.IsBusy && CurrentSelectedJobDefinition.Jobs.Any());
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            Title = appName + " (DESIGNER)";
            JobDefinitions = new ObservableCollection<JobModel>(
                new List<JobModel>
                {
                    new JobModel
                    {
                        TargetAddess = "google.de",
                        TargetPorts = new[] { 80, 443 }
                    },
                    new JobModel
                    {
                        TargetAddess = "localhost",
                        TargetPorts = new[] { 3389 }
                    }
                });
            JobDefinitionsView = CollectionViewSource.GetDefaultView(JobDefinitions) as ListCollectionView;
            base.InitDesignTimeData();
        }

        /// <inheritdoc />
        protected override void InitRuntimeData()
        {
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            Title = appName;
            ReloadOptions();
            base.InitRuntimeData();
        }

        /// <inheritdoc />
        protected override void OnInternalPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(CurrentSelectedJobDefinition))
            {
                Variables.CurrentSelectedJob = CurrentSelectedJobDefinition;
                RemoveJobCommand.RaiseCanExecuteChanged();
                StartStopJobCommand.RaiseCanExecuteChanged();
                if (CurrentSelectedJobDefinition == null && JobDefinitions.Any())
                {
                    CurrentSelectedJobDefinition = JobDefinitions.First();
                }
            }
            base.OnInternalPropertyChanged(propertyName);
        }

        private void JobDefintionsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            JobDefinitionsView.Refresh();
        }

        private void JobsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            JobsView.Refresh();
        }

        private void RunsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RunsView.Refresh();
        }

        /// <summary>
        /// Loads the options from the local file storage.
        /// </summary>
        private void ReloadOptions()
        {
            if (!File.Exists(_optionsFilePath))
            {
                return;
            }
            try
            {
                var fileContent = File.ReadAllText(_optionsFilePath);
                var settings = new JsonSerializerSettings();
                var options = JsonConvert.DeserializeObject<OptionsModel>(fileContent, settings);
                foreach (var job in options.JobDefinitions)
                {
                    JobDefinitions.Add(job);
                }
                Options = options;
                JobDefinitionsView = CollectionViewSource.GetDefaultView(JobDefinitions) as ListCollectionView;
                JobDefinitionsView.CurrentChanged += (s, e) =>
                {
                    RaisePropertyChanged(() => CurrentSelectedJobDefinition);                   
                    InitJobsView();                    
                };
                JobDefinitions.CollectionChanged += (s, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (INotifyPropertyChanged added in e.NewItems)
                        {
                            added.PropertyChanged += JobDefintionsOnPropertyChanged;
                        }
                    }
                    if (e.OldItems != null)
                    {
                        foreach (INotifyPropertyChanged removed in e.OldItems)
                        {
                            removed.PropertyChanged -= JobDefintionsOnPropertyChanged;
                        }
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Logic to hook up the <see cref="RunsView"/> for UI binding.
        /// </summary>
        private void InitRunsView()
        {
            CurrentSelectedRun = null;
            if (CurrentSelectedPingJob?.Result == null)
            {
                RunsView = null;
                return;
            }
            RunsView = CollectionViewSource.GetDefaultView(CurrentSelectedPingJob.Result.Runs) as ListCollectionView;
            RunsView.CurrentChanged += (s, e) =>
            {
                RaisePropertyChanged(() => CurrentSelectedRun);                
            };
            CurrentSelectedPingJob.Result.Runs.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged added in e.NewItems)
                    {
                        added.PropertyChanged += RunsOnPropertyChanged;
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged removed in e.OldItems)
                    {
                        removed.PropertyChanged -= RunsOnPropertyChanged;
                    }
                }
            };
            if (RunsView.Count > 0)
            {
                RunsView.MoveCurrentToFirst();
            }
        }

        /// <summary>
        /// Logic to hook up the <see cref="JobsView"/> for UI binding.
        /// </summary>
        private void InitJobsView()
        {
            CurrentSelectedPingJob = null;
            JobsView = CollectionViewSource.GetDefaultView(CurrentSelectedJobDefinition.Jobs) as ListCollectionView;
            CurrentSelectedJobDefinition.Jobs.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (INotifyPropertyChanged added in e.NewItems)
                    {
                        added.PropertyChanged += JobsOnPropertyChanged;
                    }
                }
                if (e.OldItems != null)
                {
                    foreach (INotifyPropertyChanged removed in e.OldItems)
                    {
                        removed.PropertyChanged -= JobsOnPropertyChanged;
                    }
                }
            };
            JobsView.CurrentChanged += (s, e) =>
            {
                RaisePropertyChanged(() => CurrentSelectedPingJob);                
                InitRunsView();
            };
            if (JobsView.Count > 0)
            {
                JobsView.MoveCurrentToFirst();
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Is used to open the add-new-window.
        /// </summary>
        public RelayCommand AddJobCommand { get; private set; }

        /// <summary>
        /// Is used to remove the history of runs a single job.
        /// </summary>
        public RelayCommand<JobModel> CleanJobHistoryCommand { get; private set; }

        /// <summary>
        /// Is used to react to window closing event.
        /// </summary>
        public RelayCommand ClosingCommand { get; private set; }

        /// <summary>
        /// The currently selected element of <see cref="JobDefinitions" />.
        /// </summary>
        public PingJob CurrentSelectedPingJob
        {
            get => JobsView?.CurrentItem as PingJob;
            set
            {
                JobsView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The currently selected result.
        /// </summary>
        public JobModel CurrentSelectedJobDefinition
        {
            get => JobDefinitionsView.CurrentItem as JobModel;
            set
            {
                JobDefinitionsView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The currently selected result.
        /// </summary>
        public JobSingleRunModel CurrentSelectedRun
        {
            get => RunsView?.CurrentItem as JobSingleRunModel;
            set
            {
                RunsView.MoveCurrentTo(value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Is used to react to window loaded event.
        /// </summary>
        public RelayCommand LoadedCommand { get; private set; }

        /// <summary>
        /// The currently used options.
        /// </summary>
        public OptionsModel Options { get; set; } = new OptionsModel
        {
            WindowWidth = 800,
            WindowHeight = 600,
            WindowState = WindowState.Normal
        };

        /// <summary>
        /// The bindable view of job definitions.
        /// </summary>
        public ListCollectionView JobDefinitionsView { get; private set; }

        /// <summary>
        /// The bindable view of jobs.
        /// </summary>
        public ListCollectionView JobsView { get; private set; }

        /// <summary>
        /// The bindable view of runs history.
        /// </summary>
        public ListCollectionView RunsView { get; private set; }

        /// <summary>
        /// Is used to remove one item out of the <see cref="JobDefinitions" />.
        /// </summary>
        public RelayCommand<Window> RemoveJobCommand { get; private set; }

        /// <summary>
        /// Is used to trigger writing of options back to file system.
        /// </summary>
        public RelayCommand SaveCommand { get; private set; }

        /// <summary>
        /// Is used to start or stop a single job.
        /// </summary>
        public RelayCommand<JobModel> StartStopJobCommand { get; private set; }

        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The list of job definitions available.
        /// </summary>
        private ObservableCollection<JobModel> JobDefinitions { get; set; } = new ObservableCollection<JobModel>();

        #endregion
    }
}