using SpeedOdds.UserControls.Begin;
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
    /// Interaction logic for UserControl_MainContent.xaml
    /// </summary>
    public partial class UserControl_MainContent : UserControl
    {
        public UserControl_MainContent(MainWindow mainWindow)
        {
            InitializeComponent();

            //TOP BAR
            this.ControBoxContainer.Content = new UserControl_ControlBox(mainWindow);

            //SIDE MENU
            this.MenuContainer.Content = new UserControl_Menu(this);           
        }
    }
}
