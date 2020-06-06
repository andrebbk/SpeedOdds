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

            this.MatchContainer.Content = new UserControl_AddMatch_StartTime(this);
            ChangeSubTitle("(Ataque/ Defesa)");
        }

        private void ButtonGoBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainContent.WFAPContentContainer.Content = new UserControl_Matches(_mainContent);
        }

        //PUBLIC METHODS
        public void LockMatchesUI()
        {
            ButtonGoBack.Dispatcher.BeginInvoke((Action)(() => ButtonGoBack.Visibility = Visibility.Hidden ));
            ButtonAddMatchesStartMatch.Dispatcher.BeginInvoke((Action)(() => ButtonAddMatchesStartMatch.Visibility = Visibility.Hidden));
            ButtonAddMatchesHalfTime.Dispatcher.BeginInvoke((Action)(() => ButtonAddMatchesHalfTime.Visibility = Visibility.Hidden));
        }

        public void UnlockMatchesUI()
        {
            ButtonGoBack.Dispatcher.BeginInvoke((Action)(() => ButtonGoBack.Visibility = Visibility.Visible));
            ButtonAddMatchesStartMatch.Dispatcher.BeginInvoke((Action)(() => ButtonAddMatchesStartMatch.Visibility = Visibility.Visible));
            ButtonAddMatchesHalfTime.Dispatcher.BeginInvoke((Action)(() => ButtonAddMatchesHalfTime.Visibility = Visibility.Visible));
        }

        public void ChangeSubTitle(string subTitle)
        {
            TextBoxSubTitle.Text = subTitle.Trim();
        }


        //BUTTONS
        private void ButtonAddMatchesStartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.MatchContainer.Content is UserControl_AddMatch_StartTime))
            {
                ChangeSubTitle("(Ataque/ Defesa)");
                this.MatchContainer.Content = new UserControl_AddMatch_StartTime(this);
            }                
        }

        private void ButtonAddMatchesHalfTime_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.MatchContainer.Content is UserControl_AddMatch_HalfTime))
            {
                ChangeSubTitle("(Intervalo)");
                this.MatchContainer.Content = new UserControl_AddMatch_HalfTime(this);
            }                
        }
    }
}
