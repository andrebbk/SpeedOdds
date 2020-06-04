using SpeedOdds.Commons.Helpers;
using SpeedOdds.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SpeedOdds.UserControls.MainContent
{
    /// <summary>
    /// Interaction logic for UserControl_ControlBox.xaml
    /// </summary>
    public partial class UserControl_ControlBox : UserControl
    {
        private MainWindow _mainWindow;

        public UserControl_ControlBox(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            UtilsNotification.ControlBoxController = this;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }


        //Public Methods
        public void LockControlBox()
        {
            ButtonMinimize.Dispatcher.BeginInvoke((Action)(() => ButtonMinimize.IsEnabled = false));
            ButtonExit.Dispatcher.BeginInvoke((Action)(() => ButtonExit.IsEnabled = false));
        }

        public void UnlockControlBox()
        {
            ButtonMinimize.Dispatcher.BeginInvoke((Action)(() => ButtonMinimize.IsEnabled = true));
            ButtonExit.Dispatcher.BeginInvoke((Action)(() => ButtonExit.IsEnabled = true));
        }       

    }    
}
