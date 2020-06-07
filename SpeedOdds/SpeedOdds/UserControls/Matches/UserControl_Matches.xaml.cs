using SpeedOdds.UserControls.DrawableMenu;
using SpeedOdds.UserControls.MainContent;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_Matches.xaml
    /// </summary>
    public partial class UserControl_Matches : UserControl
    {
        private UserControl_MainContent _mainContent;

        private bool IsDrawableMenuOpen;

        public UserControl_Matches(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;

            IsDrawableMenuOpen = false;
            this.DrawableMenuContainer.Content = new UserControl_DrawableMenuMatches(_mainContent);
        }

        private void ButtonOpenDrawableMenu_Click(object sender, RoutedEventArgs e)
        {
            IsDrawableMenuOpen = true;
            ButtonOpenDrawableMenu.Visibility = Visibility.Hidden;
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
            }
        }
    }
}
