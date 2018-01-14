namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using devdeer.DoctorFlox.Logic.Wpf;
    using devdeer.DoctorFlox.Logic.Wpf.Commands;

    using Logic;

    using Messages;

    using Models;

    public class AddPortViewModel : BaseViewModel
    {
        #region methods

        /// <inheritdoc />
        protected override void InitCommands()
        {
            AddPortCommand = new RelayCommand(
                () =>
                {
                    if (SelectedPort == null)
                    {
                        return;
                    }
                    MessengerInstance.Send(new PortAddInvokedMessage(SelectedPort));
                },
                () => SelectedPort != null);
            base.InitCommands();
        }

        /// <inheritdoc />
        protected override void InitData()
        {
            KnownPorts = new BindingList<PortModel>(Variables.KnownPorts?.Where(p => p.Tcp == Variables.CurrentSelectedJob.Tcp).ToList() ?? new List<PortModel>());
            base.InitData();
        }

        /// <inheritdoc />
        protected override void InitDesignTimeData()
        {
            var ports = new List<PortModel>
            {
                new PortModel
                {
                    Port = 80,
                    Purpose = "HTTP",
                    Tcp = true,
                    Udp = false
                }
            };
            KnownPorts = new BindingList<PortModel>(ports);
            base.InitDesignTimeData();
        }

        #endregion

        #region properties

        public RelayCommand AddPortCommand { get; private set; }

        public BindingList<PortModel> KnownPorts { get; private set; }

        public PortModel SelectedPort { get; set; }

        #endregion
    }
}