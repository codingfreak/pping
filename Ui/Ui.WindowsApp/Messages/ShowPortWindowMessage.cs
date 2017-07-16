namespace codingfreaks.pping.Ui.WindowsApp.Messages
{
    using System;
    using System.Linq;

    public class ShowPortWindowMessage
    {
        #region constructors and destructors

        public ShowPortWindowMessage(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        #endregion

        #region properties

        public Type ViewModelType { get; }

        #endregion
    }
}