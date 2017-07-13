namespace codingfreaks.pping.Ui.WindowsApp
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using Messages;

    using ViewModel;

    /// <summary>
    /// Is used to hook into the message bus.
    /// </summary>
    public class MessageListener
    {
        #region member vars

        public bool IsDataSource = true;

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
                    var ctx = window?.DataContext as MainViewModel;
                    if (ctx == null)
                    {
                        // strange too
                        return;
                    }
                    ctx.Jobs.Add(m.NewJob);
                });
        }

        #endregion

        #region methods

        /// <summary>
        /// Retrieves one of the currently opened windows by searching for its <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of the window.</param>
        /// <returns>The window instance or <c>null</c> if no matching window was found.</returns>
        public static Window GetWindow(Type type)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == type)
                {
                    return window as Window;
                }
            }
            return null;
        }

        #endregion
    }
}