namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;

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
                        Jobs.Remove(CurrentSelectedJob);
                        CurrentSelectedJob = null;
                    }
                },
                window => CurrentSelectedJob != null);
            ClosingCommand = new RelayCommand(
                () =>
                {
                    while (Jobs.Any(j => j.CurrentJob != null && j.CurrentJob.State == JobStateEnum.Stopping))
                    {
                        Task.Delay(100).Wait();
                    }
                    Jobs.Where(j => j.CurrentJob != null && !j.CurrentJob.Finished.HasValue).ToList().ForEach(
                        j =>
                        {
                            j.CurrentJob.Stop();                        
                        });                    
                    SaveCommand.Execute(null);
                });
            SaveCommand = new RelayCommand(
                () =>
                {
                    var options = new OptionsModel
                    {
                        JobDefinitions = Jobs.ToList()
                    };
                    var fileContent = JsonConvert.SerializeObject(options);
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
                    jobDef.StartNew();
                },
                job => CurrentSelectedJob != null);
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            Title = appName + " (DESIGNER)";
            Jobs = new BindingList<JobModel>(
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
            base.InitDesignTimeData();
        }

        /// <inheritdoc />
        protected override void InitRuntimeData()
        {
            Jobs.ListChanged += (s, e) =>
            {
                if (CurrentSelectedJob == null)
                {
                    CurrentSelectedJob = Jobs.First();
                }
            };
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            Title = appName;
            ReloadOptions();
            base.InitRuntimeData();
        }

        /// <inheritdoc />
        protected override void OnInternalPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(CurrentSelectedJob))
            {
                Variables.CurrentSelectedJob = CurrentSelectedJob;
                RemoveJobCommand.RaiseCanExecuteChanged();
                StartStopJobCommand.RaiseCanExecuteChanged();
                if (CurrentSelectedJob == null && Jobs.Any())
                {
                    CurrentSelectedJob = Jobs.First();
                }
            }
            base.OnInternalPropertyChanged(propertyName);
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
                var options = JsonConvert.DeserializeObject<OptionsModel>(fileContent);
                foreach (var job in options.JobDefinitions)
                {
                    Jobs.Add(job);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Is used to open the add-new-window.
        /// </summary>
        public RelayCommand AddJobCommand { get; private set; }

        /// <summary>
        /// Is used to react to window closing event.
        /// </summary>
        public RelayCommand ClosingCommand { get; private set; }

        /// <summary>
        /// The currently selected alement of <see cref="Jobs" />.
        /// </summary>
        public JobModel CurrentSelectedJob { get; set; }

        /// <summary>
        /// The list of job definitions available.
        /// </summary>
        public BindingList<JobModel> Jobs { get; set; } = new BindingList<JobModel>();

        /// <summary>
        /// Is used to remove one item out of the <see cref="Jobs" />.
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

        #endregion
    }
}