namespace codingfreaks.pping.Ui.WindowsApp
{
    using System;
    using System.Linq;
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using Logic;

    using Messages;

    using ViewModel;

    /// <summary>
    /// Is used to hook into the message bus.
    /// </summary>
    public class MessageListener
    {
        #region member vars

        public bool IsDataSource => true;

        #endregion

        #region constructors and destructors

        public MessageListener()
        {
            Messenger.Default.Register<AddJobWindowOpenMessage>(
                this,
                m =>
                {
                    var form = new AddJobWindow
                    {
                        Owner = GetWindow(typeof(MainWindow))
                    };
                    form.ShowDialog();
                });
            Messenger.Default.Register<JobAddedMessage>(
                this,
                m =>
                {
                    var window = GetWindow(typeof(MainWindow));
                    var ctx = window?.DataContext as MainViewsModel;
                    if (ctx == null)
                    {
                        // strange too
                        return;
                    }
                    ctx.AddJob(m.NewJob);
                });
            Messenger.Default.Register<ShowPortWindowMessage>(
                this,
                m =>
                {
                    var form = new AddPortWindow
                    {
                        Owner = GetWindow(m.ViewModelType)
                    };
                    form.Closed += (s, e) =>
                    {
                        Variables.AddPortWindow = null;
                        Messenger.Default.Send(new PortWindowClosedMessage());
                    };
                    Variables.AddPortWindow = form;
                    form.ShowDialog();                    
                });
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves one of the currently opened windows by searching for its <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type of the window.</param>
        /// <param name="searchForViewModel">If <c>true</c> the <paramref name="type" /> will be treated as the view model type.</param>
        /// <returns>The window instance or <c>null</c> if no matching window was found.</returns>
        public static Window GetWindow(Type type, bool searchForViewModel = false)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (searchForViewModel)
                {
                    if (window.DataContext.GetType() == type)
                    {
                        return window;
                    }
                }
                else
                {
                    if (window.GetType() == type)
                    {
                        return window;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}