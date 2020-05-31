using SpeedOdds.UserControls.Start;
using System.Windows;

namespace SpeedOdds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();

            //Show Start window
            this.StartContainer.Content = new UserControl_StartWindow(this);            
        }
    }
}
