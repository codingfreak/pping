namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Annotations;

    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}