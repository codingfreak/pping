namespace codingfreaks.pping.Ui.WindowsApp.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
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
        
        /// <summary>
        /// The title of the main window.
        /// </summary>
        public string Title => IsInDesignMode ? "pping (Design)" : $"[codingfreaks] pping {Version}";

        /// <summary>
        /// Retrieves the version of the current assembly in the format [MAJOR].[MINOR].[BUILD].
        /// </summary>
        public string Version
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        #endregion
    }
}