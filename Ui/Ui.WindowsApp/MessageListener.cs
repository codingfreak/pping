namespace codingfreaks.pping.Ui.WindowsApp
{
    using System;
    using System.Linq;
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using Messages;

    using ViewModel;

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
                    var form = new AddJobWindow();
                    form.ShowDialog();
                });
            Messenger.Default.Register<JobAddedMessage>(
                this,
                m =>
                {
                    var window = GetWindow(typeof(MainWindow));
                    if (window == null)
                    {
                        // strange
                        return;
                    }
                    var ctx = window.DataContext as MainViewModel;
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

        private Window GetWindow(Type type)
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