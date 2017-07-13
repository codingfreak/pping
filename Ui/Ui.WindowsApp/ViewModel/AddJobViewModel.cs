namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Linq;

    using GalaSoft.MvvmLight;

    using Models;

    public class AddJobViewModel : ViewModelBase
    {
        #region constructors and destructors

        public AddJobViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Title = "Add Job (DESIGNER)";
                FillDesignTimeData();
            }
            else
            {
                // Code runs "for real"
                Title = "Add Job";
                InitCommands();
            }
        }

        #endregion

        #region methods

        private void FillDesignTimeData()
        {
            Data = new JobModel
            {
                TargetAddess = "google.de",
                TargetPorts = new[] { 80, 443 }
            };
        }

        private void InitCommands()
        {
        }

        #endregion

        #region properties

        public JobModel Data { get; set; }

        public string Title { get; set; }

        #endregion
    }
}