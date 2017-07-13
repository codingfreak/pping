namespace codingfreaks.pping.Ui.WindowsApp
{
    using System;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region member vars

        private GridLength _lastPropertyHeight = GridLength.Auto;

        #endregion

        #region constructors and destructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region methods

        private void OnExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            _lastPropertyHeight = propertyRow.Height;
            propertyRow.Height = GridLength.Auto;
        }

        private void OnExpanderExpanded(object sender, RoutedEventArgs e)
        {
            propertyRow.Height = _lastPropertyHeight;
        }

        #endregion
    }
}