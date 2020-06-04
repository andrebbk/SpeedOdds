﻿using SpeedOdds.Commons.Helpers;
using SpeedOdds.UserControls.Competitions;
using SpeedOdds.UserControls.Teams;
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
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {

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


        //Public Methods
        public void LockMenu()
        {
            ButtonHome.Dispatcher.BeginInvoke((Action)(() => ButtonHome.IsEnabled = false));
            ButtonCompetitions.Dispatcher.BeginInvoke((Action)(() => ButtonCompetitions.IsEnabled = false));
            ButtonTeams.Dispatcher.BeginInvoke((Action)(() => ButtonTeams.IsEnabled = false));
        }

        public void UnlockMenu()
        {
            ButtonHome.Dispatcher.BeginInvoke((Action)(() => ButtonHome.IsEnabled = true));
            ButtonCompetitions.Dispatcher.BeginInvoke((Action)(() => ButtonCompetitions.IsEnabled = true));
            ButtonTeams.Dispatcher.BeginInvoke((Action)(() => ButtonTeams.IsEnabled = true));
        }
    }
}
