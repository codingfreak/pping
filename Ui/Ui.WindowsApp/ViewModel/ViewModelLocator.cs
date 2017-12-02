namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    using System;
    using System.Linq;

    using Autofac;

    using Logic;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        #region properties

        public AddJobViewModel AddJob => Variables.AutoFacContainer.Resolve<AddJobViewModel>();

        public AddPortViewModel AddPort => Variables.AutoFacContainer.Resolve<AddPortViewModel>();

        public MainViewsModel Main => Variables.AutoFacContainer.Resolve<MainViewsModel>();

        #endregion
    }
}