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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedOdds.UserControls.Competitions
{
    /// <summary>
    /// Interaction logic for UserControl_Competitions.xaml
    /// </summary>
    public partial class UserControl_Competitions : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_Competitions(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
        }

        private void ButtonManageSeasons_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Seasons)) 
                _mainContent.WFAPContentContainer.Content = new UserControl_Seasons(_mainContent);
        }

        //BUTTONS
        private void ButtonSaveCompetition_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonRemoveSeason_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
