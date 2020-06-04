using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpeedOdds.UserControls.MainContent;

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_AddMatches.xaml
    /// </summary>
    public partial class UserControl_AddMatches : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_AddMatches(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;

            this.MatchContainer.Content = new UserControl_AddMatch_StartTime();
        }

        private void ButtonGoBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_Matches(_mainContent);
        }
    }
}
