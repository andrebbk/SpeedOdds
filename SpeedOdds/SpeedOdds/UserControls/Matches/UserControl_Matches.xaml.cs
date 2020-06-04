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

namespace SpeedOdds.UserControls.Matches
{
    /// <summary>
    /// Interaction logic for UserControl_Matches.xaml
    /// </summary>
    public partial class UserControl_Matches : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_Matches(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_AddMatches(_mainContent);
        }
    }
}
