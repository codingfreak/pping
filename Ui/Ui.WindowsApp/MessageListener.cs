namespace codingfreaks.pping.Ui.WindowsApp
{
    using System;
    using System.Linq;
    using System.Windows;

    using GalaSoft.MvvmLight.Messaging;

    using Messages;

    public class MessageListener
    {
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
        }

        #endregion

        #region properties

        public bool IsDataSource = true;

        #endregion
    }
}