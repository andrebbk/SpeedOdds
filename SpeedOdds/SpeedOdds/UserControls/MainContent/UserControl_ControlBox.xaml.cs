using SpeedOdds.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.WindowState = WindowState.Minimized;
        }

    }    
}
