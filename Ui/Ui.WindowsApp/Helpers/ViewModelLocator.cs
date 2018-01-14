namespace codingfreaks.pping.Ui.WindowsApp.Helpers
{
    using System;
    using System.Linq;

    using Autofac;

    using ViewModels;

    /// <summary>
    /// Acts as a factory for view models.
    /// </summary>
    public class ViewModelLocator
    {
        #region properties

        /// <summary>
        /// Returns a fresh instance of a main view model.
        /// </summary>
        public MainViewModel MainViewModel => Variables.AutoFacContainer.Resolve<MainViewModel>();

        #endregion
    }
}