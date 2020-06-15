using SpeedOdds.Commons.Helpers;
using SpeedOdds.UserControls.DrawableMenu;
using SpeedOdds.UserControls.MainContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedOdds.UserControls.HomeAwayTeams
{
    /// <summary>
    /// Interaction logic for UserControl_HomeAwayTeams.xaml
    /// </summary>
    public partial class UserControl_HomeAwayTeams : UserControl
    {
        private UserControl_MainContent _mainContent;

        private bool IsDrawableMenuOpen;

        public UserControl_HomeAwayTeams(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                _mainContent = mainContent;

                IsDrawableMenuOpen = false;
                DrawableMenuContainer.Dispatcher.BeginInvoke((Action)(() =>
                    DrawableMenuContainer.Content = new UserControl_DrawableMenuTeams(_mainContent, this)));

                UtilsNotification.StopLoadingAnimation();

            }).Start();
        }


        //BUTTONS
        private void ButtonOpenDrawableMenu_Click(object sender, RoutedEventArgs e)
        {
            IsDrawableMenuOpen = true;
            ButtonOpenDrawableMenu.Visibility = Visibility.Hidden;

            //Canvas.SetZIndex(DataGridMatches, -1);
        }


        //EVENTS        
        private void GridBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Click on background

            if (IsDrawableMenuOpen)
            {
                Storyboard sb = this.FindResource("CloseDrawableMenu") as Storyboard;
                sb.Begin();

                IsDrawableMenuOpen = false;

                ButtonOpenDrawableMenu.Visibility = Visibility.Visible;

                //Canvas.SetZIndex(DataGridMatches, 0);
            }
        }
    }
}
