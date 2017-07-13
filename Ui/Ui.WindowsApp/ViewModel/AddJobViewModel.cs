namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using cfUtils.Logic.Wpf.MvvmLight;

    using GalaSoft.MvvmLight.Command;

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

        protected override void InitCommands()
        {
            OkCommand = new RelayCommand<Window>(
                window =>
                {
                    MessengerInstance.Send(new JobAddedMessage(Data));
                    window.Close();
                },
                window => Data.IsValid);
        }

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

        protected override void InitRuntimeData()
        {
            Title = "Add Job";
            base.InitRuntimeData();
        }

        #endregion

        #region properties

        public JobModel Data { get; set; } = new JobModel();

        public RelayCommand<Window> OkCommand { get; private set; }

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