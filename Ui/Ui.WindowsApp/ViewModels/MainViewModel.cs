namespace codingfreaks.pping.Ui.WindowsApp.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading;

    using devdeer.DoctorFlox;
    using devdeer.DoctorFlox.Interfaces;

    using Views;

    /// <summary>
    /// View model for the <see cref="MainWindow" />.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region constructors and destructors

        public MainViewModel()
        {
        }

        public MainViewModel(IMessenger messenger) : base(messenger)
        {
        }

        public MainViewModel(IMessenger messenger, SynchronizationContext synchronizationContext) : base(messenger, synchronizationContext)
        {
        }

        #endregion

        #region properties

        public string Title => "pping";

        #endregion
    }
}