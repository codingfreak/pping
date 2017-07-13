namespace codingfreaks.pping.Ui.WindowsApp.Models
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Annotations;

    /// <summary>
    /// Abstract base class for all models.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        #region events

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region methods

        /// <summary>
        /// Helper method to fire <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property (is automatically inserted).</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}