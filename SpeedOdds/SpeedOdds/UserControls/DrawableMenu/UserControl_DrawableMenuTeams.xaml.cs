using SpeedOdds.Models.Shared;
using SpeedOdds.Services;
using SpeedOdds.UserControls.HomeAwayTeams;
using SpeedOdds.UserControls.MainContent;
using SpeedOdds.UserControls.Matches;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SpeedOdds.UserControls.DrawableMenu
{
    /// <summary>
    /// Interaction logic for UserControl_DrawableMenuTeams.xaml
    /// </summary>
    public partial class UserControl_DrawableMenuTeams : UserControl
    {
        private UserControl_MainContent _mainContent;
        private UserControl_HomeAwayTeams _parent;
        private CompetitionService competitionService;
        private TeamService teamService;

        public UserControl_DrawableMenuTeams(UserControl_MainContent mainContent, UserControl_HomeAwayTeams parent)
        {
            InitializeComponent();
            _mainContent = mainContent;
            _parent = parent;
            _parent.child = this; //Important to connect both aways
            competitionService = new CompetitionService();
            teamService = new TeamService();
            InitFilterForm();

        }

        //FUNCTIONS
        public void InitFilterForm()
        {
            //Load competitions
            var competitionList = competitionService.GetCompetitions();

            ObservableCollection<CompetitionComboModel> compsBox = new ObservableCollection<CompetitionComboModel>();
            compsBox.Add(new CompetitionComboModel()
            {
                CompetitionId = 0,
                CompetitionName = "Selecionar competição"
            });

            foreach (var item in competitionList)
                compsBox.Add(new CompetitionComboModel()
                {
                    CompetitionId = item.CompetitionId,
                    CompetitionName = item.Name + " - " + competitionService.GetCompetitionSeasonName(item.CompetitionId)
                });

            ComboBoxCompetition.ItemsSource = compsBox;
            ComboBoxCompetition.SelectedValue = compsBox.FirstOrDefault();

            ComboBoxTeam.IsEnabled = false;
        }



        //PUBLIC
        public int? GetCompetitionValue()
        {
            if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null)
            {
                if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId != 0)
                    return ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;
            }

            return null;
        }

        public int? GetTeamValue()
        {
            if (((TeamComboModel)ComboBoxTeam.SelectedItem) != null)
            {
                if (((TeamComboModel)ComboBoxTeam.SelectedItem).TeamId != 0)
                    return ((TeamComboModel)ComboBoxTeam.SelectedItem).TeamId;
            }

            return null;
        }


        //BUTTONS
        private void ButtonApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            _parent.LoadGridWithCalculatedMatchesData(GetCompetitionValue(), GetTeamValue());
        }


        //EVENT VALIDATIONS
        private void ComboBoxCompetition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem) != null)
            {
                if (((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId != 0)
                {
                    int compId = ((CompetitionComboModel)ComboBoxCompetition.SelectedItem).CompetitionId;
                    new Thread(() =>
                    {
                        //Load teams
                        var teamList = teamService.GetCompetitionTeams(compId);

                        if (teamList != null && teamList.Count() > 0)
                        {
                            ObservableCollection<TeamComboModel> teamsBox = new ObservableCollection<TeamComboModel>();
                            teamsBox.Add(new TeamComboModel()
                            {
                                TeamId = 0,
                                TeamName = "Selecionar equipa"
                            });

                            foreach (var item in teamList) teamsBox.Add(new TeamComboModel() { TeamId = item.TeamId, TeamName = item.Name });

                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.ItemsSource = teamsBox));
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.SelectedValue = teamsBox.FirstOrDefault()));

                            //Active combobox
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.IsEnabled = true));
                        }
                        else
                        {
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.ItemsSource = null));
                            ComboBoxTeam.Dispatcher.BeginInvoke((Action)(() => ComboBoxTeam.IsEnabled = false));
                        }

                    }).Start();
                }
                else
                    ComboBoxTeam.IsEnabled = false;
            }
        }
    }
}
