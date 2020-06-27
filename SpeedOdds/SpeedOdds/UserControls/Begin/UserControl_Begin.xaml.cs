using SpeedOdds.Commons.Helpers;
using SpeedOdds.Services;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedOdds.UserControls.Begin
{
    /// <summary>
    /// Interaction logic for UserControl_Begin.xaml
    /// </summary>
    public partial class UserControl_Begin : UserControl
    {
        private UserControl_MainContent _mainContent;
        private SeasonService seasonService;
        private CompetitionService competitionService;
        private TeamService teamService;
        private MatchService matchService;

        public UserControl_Begin(UserControl_MainContent mainContent)
        {
            InitializeComponent();

            new Thread(() =>
            {
                UtilsNotification.StartLoadingAnimation();

                _mainContent = mainContent;
                seasonService = new SeasonService();
                competitionService = new CompetitionService();
                teamService = new TeamService();
                matchService = new MatchService();

                Thread.Sleep(1000);

                TextBoxNSeasons.Dispatcher.BeginInvoke((Action)(() => TextBoxNSeasons.Text = seasonService.GetSeasonsNumber().ToString()));
                TextBoxNCompetitions.Dispatcher.BeginInvoke((Action)(() => TextBoxNCompetitions.Text = competitionService.GetCompetitionsNumber().ToString()));
                TextBoxNTeams.Dispatcher.BeginInvoke((Action)(() => TextBoxNTeams.Text = teamService.GetTeamsNumber().ToString()));
                TextBoxNMatches.Dispatcher.BeginInvoke((Action)(() => TextBoxNMatches.Text = matchService.GetMatchesNumber().ToString()));

                UtilsNotification.StopLoadingAnimation();
            }).Start();            
        }
    }
}
