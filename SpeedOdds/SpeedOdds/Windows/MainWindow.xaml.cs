using SpeedOdds.UserControls.MainContent;
using System;
using System.Windows;

namespace SpeedOdds.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(StartWindow startWindow)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;

            InitializeComponent();

            //Mostrar a barra de tarefas do windows
            this.MaxHeight = SystemParameters.WorkArea.Height + 14;

            //Close start window
            startWindow.Dispatcher.BeginInvoke((Action)(
                 () => startWindow.Close()
             ));

            //load content
            this.MainWindowContainer.Content = new UserControl_MainContent(this);
        }
    }
}
