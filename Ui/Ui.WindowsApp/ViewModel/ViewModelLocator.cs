using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace codingfreaks.pping.Ui.WindowsApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                
            }
            else
            {
                // Create run time view services and models
                
            }
            SimpleIoc.Default.Register<MainViewsModel>();
            SimpleIoc.Default.Register<AddJobViewModel>();
            SimpleIoc.Default.Register<AddPortViewModel>();
        }

        public MainViewsModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewsModel>();
            }
        }

        public AddJobViewModel AddJob
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddJobViewModel>();
            }
        }

        public AddPortViewModel AddPort
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddPortViewModel>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}