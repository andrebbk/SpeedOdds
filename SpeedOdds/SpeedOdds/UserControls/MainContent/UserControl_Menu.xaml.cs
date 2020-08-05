using SpeedOdds.Commons.Helpers;
using SpeedOdds.UserControls.Begin;
using SpeedOdds.UserControls.Competitions;
using SpeedOdds.UserControls.HomeAwayTeams;
using SpeedOdds.UserControls.Matches;
using SpeedOdds.UserControls.Rankings;
using SpeedOdds.UserControls.Teams;
using SpeedOdds.UserControls.TimeGoals;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SpeedOdds.UserControls.MainContent
{
    /// <summary>
    /// Interaction logic for UserControl_Menu.xaml
    /// </summary>
    public partial class UserControl_Menu : UserControl
    {
        private UserControl_MainContent _mainContent;

        public UserControl_Menu(UserControl_MainContent mainContent)
        {
            InitializeComponent();
            _mainContent = mainContent;

            UtilsNotification.MenuController = this;
            UtilsNotification.LoadingLottieAnimationView = LottieAnimationView;
            UtilsNotification.TextBoxLoading = TextBoxLoading;

            //BEGIN
            if (!(mainContent.WFAPContentContainer.Content is UserControl_Begin))
                mainContent.WFAPContentContainer.Content = new UserControl_Begin(mainContent);
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Begin))
                _mainContent.WFAPContentContainer.Content = new UserControl_Begin(_mainContent);
        }

        private void ButtonCompetitions_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Competitions))
                _mainContent.WFAPContentContainer.Content = new UserControl_Competitions(_mainContent);
        }

        private void ButtonTeams_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Teams))
                _mainContent.WFAPContentContainer.Content = new UserControl_Teams(_mainContent);
        }

        private void ButtonMatches_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Matches))
                _mainContent.WFAPContentContainer.Content = new UserControl_Matches(_mainContent);
        }

        private void ButtonRegisterMatches_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_AddMatches))
                _mainContent.WFAPContentContainer.Content = new UserControl_AddMatches(_mainContent);
        }

        private void ButtonHomeAwayTeams_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_HomeAwayTeams))
                _mainContent.WFAPContentContainer.Content = new UserControl_HomeAwayTeams(_mainContent);
        }



        //Public Methods
        public void LockMenu()
        {
            ButtonHome.Dispatcher.BeginInvoke((Action)(() => ButtonHome.IsEnabled = false));
            ButtonCompetitions.Dispatcher.BeginInvoke((Action)(() => ButtonCompetitions.IsEnabled = false));
            ButtonTeams.Dispatcher.BeginInvoke((Action)(() => ButtonTeams.IsEnabled = false));
            ButtonMatches.Dispatcher.BeginInvoke((Action)(() => ButtonMatches.IsEnabled = false));
            ButtonRegisterMatches.Dispatcher.BeginInvoke((Action)(() => ButtonRegisterMatches.IsEnabled = false));
            ButtonHomeAwayTeams.Dispatcher.BeginInvoke((Action)(() => ButtonHomeAwayTeams.IsEnabled = false));
            ButtonTimeGoals.Dispatcher.BeginInvoke((Action)(() => ButtonTimeGoals.IsEnabled = false));
            ButtonRanks.Dispatcher.BeginInvoke((Action)(() => ButtonRanks.IsEnabled = false));
        }

        public void UnlockMenu()
        {
            ButtonHome.Dispatcher.BeginInvoke((Action)(() => ButtonHome.IsEnabled = true));
            ButtonCompetitions.Dispatcher.BeginInvoke((Action)(() => ButtonCompetitions.IsEnabled = true));
            ButtonTeams.Dispatcher.BeginInvoke((Action)(() => ButtonTeams.IsEnabled = true));
            ButtonMatches.Dispatcher.BeginInvoke((Action)(() => ButtonMatches.IsEnabled = true));
            ButtonRegisterMatches.Dispatcher.BeginInvoke((Action)(() => ButtonRegisterMatches.IsEnabled = true));
            ButtonHomeAwayTeams.Dispatcher.BeginInvoke((Action)(() => ButtonHomeAwayTeams.IsEnabled = true));
            ButtonTimeGoals.Dispatcher.BeginInvoke((Action)(() => ButtonTimeGoals.IsEnabled = true));
            ButtonRanks.Dispatcher.BeginInvoke((Action)(() => ButtonRanks.IsEnabled = true));
        }

        private void ButtonTimeGoals_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_TimeGoals))
                _mainContent.WFAPContentContainer.Content = new UserControl_TimeGoals(_mainContent);
        }

        private void ButtonRanks_Click(object sender, RoutedEventArgs e)
        {
            if (!(_mainContent.WFAPContentContainer.Content is UserControl_Rankings))
                _mainContent.WFAPContentContainer.Content = new UserControl_Rankings(_mainContent);
        }
    }
}
