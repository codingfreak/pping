namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using cfUtils.Logic.Portable.Extensions;
    using cfUtils.Logic.Wpf.MvvmLight;

    using GalaSoft.MvvmLight.Command;

    using Logic;

    using Messages;

    using Models;

    /// <summary>
    /// Defines the logic and data of the <see cref="AddJobWindow" />.
    /// </summary>
    public class AddJobViewModel : BaseViewModel
    {
        #region member vars

        private string _targetPorts = string.Empty;

        #endregion

        #region methods

        /// <inheritdoc />
        protected override void InitCommands()
        {
            OkCommand = new RelayCommand<Window>(
                window =>
                {
                    MessengerInstance.Send(new JobAddedMessage(Data));
                    window.Close();
                },
                window => Data.IsValid);
            ShowPortSelectCommand = new RelayCommand(
                () =>
                {
                    MessengerInstance.Send(new ShowPortWindowMessage(GetType()));
                },
                () => Variables.AddPortWindow == null);
        }
        

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            Title = "Add Job (DESIGNER)";
            Data = new JobModel
            {
                TargetAddess = "google.de",
                TargetPorts = new[] { 80, 443 }
            };
            base.InitDesignTimeData();
        }

        /// <inheritdoc />
        protected override void InitMessenger()
        {
            MessengerInstance.Register<PortWindowClosedMessage>(
                this,
                m =>
                {
                    ShowPortSelectCommand.RaiseCanExecuteChanged();
                });
            MessengerInstance.Register<PortAddInvokedMessage>(
                this,
                m =>
                {
                    var ports = TargetPorts.IsNullOrEmpty() ? Enumerable.Empty<int>() : TargetPorts.Split(',').Select(p => int.Parse(p));
                    if (ports.Contains(m.PortModel.Port))
                    {
                        return;
                    }
                    TargetPorts += (TargetPorts.IsNullOrEmpty() || TargetPorts.EndsWith(",") ? string.Empty : ",") + m.PortModel.Port.ToString();
                    OkCommand.RaiseCanExecuteChanged();
                });
            base.InitMessenger();
        }

        protected override void InitRuntimeData()
        {
            Title = "Add Job";
            Data = new JobModel();
            Variables.CurrentSelectedJob = Data;
            base.InitRuntimeData();
        }

        #endregion

        #region properties

        public JobModel Data { get; private set; } 

        public RelayCommand<Window> OkCommand { get; private set; }

        public RelayCommand ShowPortSelectCommand { get; private set; }

        public string TargetPorts
        {
            get => _targetPorts;
            set
            {
                if (value == _targetPorts)
                {
                    return;
                }
                try
                {
                    var error = false;
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
                                error = true;
                                break;
                            }
                        }
                    }
                    if (!error)
                    {
                        Data.TargetPorts = result;
                    }
                    _targetPorts = value;
                    RaisePropertyChanged(() => TargetPorts);
                    OkCommand.RaiseCanExecuteChanged();
                }
                catch
                {
                    // empty catch                    
                }
            }
        }

        public string Title { get; set; }

        #endregion
    }
}