namespace codingfreaks.pping.Ui.WindowsApp.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Enumerations;

    /// <summary>
    /// Interaction logic for StateDisplayControl.xaml
    /// </summary>
    public partial class StateDisplayControl : UserControl
    {
        #region constants

        public static readonly DependencyProperty CurrentStateProperty =
            DependencyProperty.Register("CurrentState", typeof(JobStateEnum), typeof(StateDisplayControl), new FrameworkPropertyMetadata(JobStateEnum.Unkown, OnPropertyChanged));

        #endregion

        #region constructors and destructors

        public StateDisplayControl()
        {
            InitializeComponent();
        }

        #endregion

        #region methods

        private static void OnPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var control = source as StateDisplayControl;
            if (control == null)
            {
                return;
            }
            var state = (JobStateEnum)e.NewValue;
            control.runningContainer.Visibility = state == JobStateEnum.Running ? Visibility.Visible : Visibility.Collapsed;
            control.waitingContainer.Visibility = state == JobStateEnum.Waiting ? Visibility.Visible : Visibility.Collapsed;
            control.stoppingContainer.Visibility = state == JobStateEnum.Stopping ? Visibility.Visible : Visibility.Collapsed;

        }

        #endregion

        #region properties

        public JobStateEnum CurrentState { get; set; }

        #endregion
    }
}